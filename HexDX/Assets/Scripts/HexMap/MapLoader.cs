using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class MapLoader : MonoBehaviour {
    private static readonly string mapsDir = "Maps/";
    public string csvMapFile;

    public HexMap battleMap;
    private HexDimension hexDimension;

    void Awake() {
        battleMap = GetComponent<HexMap>();
        hexDimension = GetComponent<HexDimension>();

        ////// DEBUG CODE //////
        if (battleMap == null) {
            Debug.Log("BattleMap needs to be set -> MapLoader.cs");
        }
        if (hexDimension == null) {
            Debug.Log("HexDimension needs to be set -> MapLoader.cs");
        }
        ////////////////////////
    }

    public void LoadMap() {
        if (battleMap != null && hexDimension != null) {
            LevelManager lm = LevelManager.activeInstance;
            if (lm) {
                csvMapFile = lm.GetCurrentSceneFile();
            }
            LoadHexMap(csvMapFile);
            Debug.Log("Finished Loading Map");
        }
    }

    private void LoadHexMap(string hexMapFile) {
        string[] mapCsvRows = GameResources.GetFileLines(mapsDir + hexMapFile);
        battleMap.ClearMap();
        int currentLine = 0;

        string[] dimensionRow = mapCsvRows[currentLine++].Split(',');
        if (dimensionRow.Length != 2) Debug.Log("ERROR :: INVALID FORMAT FOR LEVEL FILE");
        int rows = Convert.ToInt32(dimensionRow[0]);
        int cols = Convert.ToInt32(dimensionRow[1]);
        LoadMapTiles(mapCsvRows, rows);
        currentLine += rows;

        CustomUnitLoader unitLoader = BattleManager.instance.unitLoader;
        if (unitLoader && unitLoader.CanLoadUnits()) {
            unitLoader.LoadUnits();
        } else if (currentLine < mapCsvRows.Length){
            /* Load enemy unit if specified */
            int numUnits = Convert.ToInt32(mapCsvRows[currentLine++]);
            LoadEnemyUnits(mapCsvRows, currentLine, numUnits);
            currentLine += numUnits;

            if(currentLine < mapCsvRows.Length) {
                /* Load deployment zone if specified */
                int numDep = Convert.ToInt32(mapCsvRows[currentLine++]);
                LoadDeploymentZone(mapCsvRows, currentLine, numDep);
            }
        }
    }

    private void LoadMapTiles(string[] mapCsvRows, int numRows) {
        List<Tile> row;
        float x = 0;
        float y = 0;
        float z = 0;
        for (int rowIndex = 0; rowIndex < numRows; rowIndex++) {
            ////// DEBUG CODE //////
            GameObject rowObj = CreateNewRowObj(rowIndex);
            ////////////////////////
            row = new List<Tile>();
            int columnIndex = 0;
            string[] line = mapCsvRows[rowIndex + 1].Split(',');
            foreach (string num in line) {
                GameObject tileObj = CreateTile(int.Parse(num), new Vector3(x, y, z), rowObj, rowIndex, columnIndex);
                Tile newTile = tileObj != null ? tileObj.GetComponent<Tile>() : null;
                row.Add(newTile);
                x += 2 * hexDimension.width;
                columnIndex++;
            }
            HexMap.mapArray.Add(row);
            y -= 2 * hexDimension.apex - hexDimension.minorApex;
            x -= 2 * hexDimension.width * line.Length - hexDimension.width;
            z -= .001f;
        }
    }

    // Debug/organizational purposes - can be removed later
    private GameObject CreateNewRowObj(int rowIndex) {
        GameObject rowObj = new GameObject("Row " + rowIndex);
        rowObj.transform.parent = battleMap.transform;
        rowObj.transform.localPosition = new Vector3(0, 0, 0);
        return rowObj;
    }

    private GameObject CreateTile(int val, Vector3 pos, GameObject rowObj, int row, int col) {
        int type = val / 100;
        int variant = val % 100;
        GameObject tileObj = GameResources.InstantiateTile(type, variant);
        if (tileObj != null) {
            tileObj.name = string.Format("Tile ({0}, {1})", row, col);
            tileObj.transform.parent = rowObj.transform;
            tileObj.transform.localPosition = pos;
            Animator animator = tileObj.GetComponent<Animator>();
            if (animator)
            {
               Timing.RunCoroutine(OffsetTile(tileObj.GetComponent<Tile>(), (float)col % 6.0f / 6.0f));
            }
            TileLocation location = tileObj.GetComponent<TileLocation>();
            location.col = col;
            location.row = row;
        }
        return tileObj;
    }

    private IEnumerator<float> OffsetTile(Tile tile, float t)
    {
        yield return Timing.WaitForSeconds(t);
        RuntimeAnimatorController controller =tile.GetComponent<Animator>().runtimeAnimatorController;
        tile.GetComponent<Animator>().runtimeAnimatorController = tile.animations[(tile.animations.IndexOf(controller)+1)%tile.animations.Count];
        tile.GetComponent<Animator>().runtimeAnimatorController = controller;
    }

    private void LoadEnemyUnits(string[] mapCvsLines, int startLineIndex, int numUnits) {
        //Debug.Log("Number Of Units :: " + numUnits);
        for (int i = 0; i < numUnits; i++) {
            string[] data = mapCvsLines[startLineIndex + i].Split(',');
            int unitRow = Convert.ToInt32(data[0]);
            int unitCol = Convert.ToInt32(data[1]);
            int veterancy = Convert.ToInt32(data[2]);
            int health = Convert.ToInt32(data[3]);
            int attack = Convert.ToInt32(data[4]);
            int power = Convert.ToInt32(data[5]);
            int defense = Convert.ToInt32(data[6]);
            int resist = Convert.ToInt32(data[7]);
            int mobID = Convert.ToInt32(data[8]);
            int mobType = Convert.ToInt32(data[9]);
            int direction = Convert.ToInt32(data[10]);
            string type = data[data.Length - 1].Trim();
            GameObject unitObject = null;
            Debug.Log(type);
            unitObject = Instantiate(Resources.Load("Units/" + type)) as GameObject;
            Unit unit = unitObject.GetComponent<Unit>();
            // Read in the stats //
            UnitStats stats = unitObject.GetComponent<UnitStats>();
            stats.maxHealth = health;
            stats.health = health;
            //Debug.Log("Health: " + stats.health);
            stats.veterancy = veterancy;
            stats.attack = attack;
            stats.power = power;
            stats.defense = defense;
            stats.resistance = resist;
            stats.className = type;
            ///////////////////////
            unit.SetTile(HexMap.mapArray[unitRow][unitCol]);
            unit.facing = direction;
            // HEURISTIC AI TEST //
            unitObject.AddComponent<HeuristicAI>();

            //unitObject.AddComponent<BasicUnitAI>();
        }
        //Debug.Log("LoadedEnemies");
    }

    private void LoadDeploymentZone(string[] mapCvsLines, int startLineIndex, int numDep) {
        //Debug.Log("Number Of Deployment Zones :: " + numDep);
        // Read deployment tiles
        List<Tile> deployZone = new List<Tile>();
        for (int i = 0; i < numDep; i++) {
            string[] data = mapCvsLines[startLineIndex + i].Split(',');
            int depRow = Convert.ToInt32(data[0]);
            int depCol = Convert.ToInt32(data[1]);
            deployZone.Add(HexMap.mapArray[depRow][depCol]);
        }

        List<Unit> activeUnits = GameManager.instance.activeUnits;
        if (activeUnits != null && deployZone.Count > 0) {
            int limit = activeUnits.Count;
            if (limit > deployZone.Count) {
                Debug.Log("Warning: Fewer deployent tiles then active units. Loading fewer units than in active army.");
                limit = deployZone.Count;
            }

            // Add units to deployment tiles
            for(int i = 0; i < limit; i++) {
                activeUnits[i].SetTile(deployZone[i]);
            }

            // Pass tiles to deployment controller
            DeploymentController deployController = BattleManager.instance.deploymentController;
            if (deployController) {
                deployController.LoadDeploymentTiles(deployZone);
            }
        } else {
            Debug.Log("Either no active units or deployment tiles registered.");
        }
    }
}

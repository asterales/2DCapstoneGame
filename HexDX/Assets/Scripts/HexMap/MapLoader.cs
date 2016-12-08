using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class MapLoader : MonoBehaviour {
    private static readonly string mapsDir = "Maps/";
    public string csvMapFile;
    public Dictionary<int, Mob> mobList;
    public HexMap battleMap;
    private HexDimension hexDimension;

    void Awake() {
        battleMap = GetComponent<HexMap>();
        hexDimension = GetComponent<HexDimension>();
        mobList = new Dictionary<int, Mob>();
        ////// DEBUG CODE //////
        if (battleMap == null) {
            Debug.Log("BattleMap needs to be set -> MapLoader.cs");
        }
        if (hexDimension == null) {
            Debug.Log("HexDimension needs to be set -> MapLoader.cs");
        }
        ////////////////////////
    }

    public void LoadMap(CustomUnitLoader unitLoader = null) {
        if (battleMap != null && hexDimension != null) {
            Debug.Log("MapLoader.cs - Loading Map: " + csvMapFile);
            LoadHexMap(csvMapFile, unitLoader);
            Debug.Log("MapLoader.cs - Finished Loading Map");
        }
    }

    private void LoadHexMap(string hexMapFile, CustomUnitLoader unitLoader = null) {
        string[] mapCsvRows = GameResources.GetFileLines(mapsDir + hexMapFile);
        battleMap.ClearMap();
        int currentLine = 0;

        string[] levelTypeRow = mapCsvRows[currentLine++].Split(','); // load victtory condition at the end

        string[] dimensionRow = mapCsvRows[currentLine++].Split(',');
        Debug.Log(dimensionRow[0] + " " + dimensionRow[1]);
        if (dimensionRow.Length != 2) Debug.Log("ERROR :: INVALID FORMAT FOR LEVEL FILE");
        int rows = Convert.ToInt32(dimensionRow[0]);
        int cols = Convert.ToInt32(dimensionRow[1]);
        LoadMapTiles(mapCsvRows, rows, currentLine);
        currentLine += rows;

        List<Unit> enemyUnits = null;
        if (unitLoader && unitLoader.CanLoadUnits()) {
            unitLoader.LoadUnits();
            enemyUnits = unitLoader.GetEnemyUnits();
        } else if (currentLine < mapCsvRows.Length){
            /* Load enemy unit if specified */
            int numUnits = Convert.ToInt32(mapCsvRows[currentLine++]);
            enemyUnits = LoadEnemyUnits(mapCsvRows, currentLine, numUnits);
            currentLine += numUnits;

            if(currentLine < mapCsvRows.Length) {
                /* Load deployment zone if specified */
                int numDep = Convert.ToInt32(mapCsvRows[currentLine++]);
                LoadDeploymentZone(mapCsvRows, currentLine, numDep);
            }
        }

        // load victory condition after tiles and units loaded
        LoadVictoryCondition(levelTypeRow, enemyUnits);
    }

    // Victory Conditions
    // 0: Rout
    // 1: Survive
    // 2: Assasin

    // For Survive ::
    // 1,x where x is the number of turns

    // For Assasin ::
    // 2,x where x is the index of desired enemy
    private void LoadVictoryCondition(string[] vcTokens, List<Unit> enemyUnits) {
        int vcType = Convert.ToInt32(vcTokens[0]);
        VictoryCondition vc;
        switch(vcType) {
            case 0:
                Annihilation routVC = gameObject.AddComponent<Annihilation>();
                routVC.ai = BattleManager.instance.ai;
                vc = routVC;
                break;
            case 1:
                Survive surviveVC = gameObject.AddComponent<Survive>();
                surviveVC.numTurns = Convert.ToInt32(vcTokens[1]);
                vc = surviveVC;
                break;
            case 2:
                KillCommander killVC = gameObject.AddComponent<KillCommander>();
                killVC.commander = enemyUnits[Convert.ToInt32(vcTokens[1])];
                vc = killVC;
                break;
            default:
                Annihilation defaultVC = gameObject.AddComponent<Annihilation>();
                defaultVC.ai = BattleManager.instance.ai;
                vc = defaultVC;
                break;
        }
        Debug.Log("MapLoader.cs - Loaded victory condition: " + vc.GetType());
    }

    private void LoadMapTiles(string[] mapCsvRows, int numRows, int start) {
        Debug.Log("MapLoader.cs - Loading Map Tiles");
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
            string[] line = mapCsvRows[rowIndex + start].Split(',');
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
            if (animator) {
               Timing.RunCoroutine(OffsetTile(tileObj.GetComponent<Tile>(), (float)col % 6.0f / 6.0f));
            }
            TileLocation location = tileObj.GetComponent<TileLocation>();
            location.col = col;
            location.row = row;
        }
        return tileObj;
    }

    private IEnumerator<float> OffsetTile(Tile tile, float t) {
        yield return Timing.WaitForSeconds(t);
        RuntimeAnimatorController controller =tile.GetComponent<Animator>().runtimeAnimatorController;
        tile.GetComponent<Animator>().runtimeAnimatorController = tile.animations[(tile.animations.IndexOf(controller)+1)%tile.animations.Count];
        tile.GetComponent<Animator>().runtimeAnimatorController = controller;
    }

    private List<Unit> LoadEnemyUnits(string[] mapCvsLines, int startLineIndex, int numUnits) {
        Debug.Log("MapLoader.cs - Loading Enemy Units - Number Of Units :: " + numUnits);
        List<Unit> enemiesLoaded = new List<Unit>();
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
            UnitAI ai = unitObject.AddComponent<NewAI>();
            Mob mob = null;
            if (!mobList.ContainsKey(mobID))
            {
                mob = Mob.GetMob(mobType);
                mobList.Add(mobID, mob);
            }
            else
            {
                mob = mobList[mobID];
            }
            if (mob != null)
                mob.addMember(unit);
            ai.mob = mob;
            enemiesLoaded.Add(unit);
            //unitObject.AddComponent<BasicUnitAI>();
        }
        //Debug.Log("LoadedEnemies");
        return enemiesLoaded;
    }

    private void LoadDeploymentZone(string[] mapCvsLines, int startLineIndex, int numDep) {
        Debug.Log("MapLoader.cs - Loading deployment zone - Number Of Deployment Zones :: " + numDep);
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

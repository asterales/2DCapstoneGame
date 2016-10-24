using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MapLoader : MonoBehaviour {
    private static readonly string mapsDir = "Maps/";
    public string csvMapFile;
    public bool isTutorial;

    private HexMap battleMap;
    private HexDimension hexDimension;
    private ScriptedAIBattleController scriptedAI;

    void Awake() {
        battleMap = this.gameObject.GetComponent<HexMap>();
        hexDimension = this.gameObject.GetComponent<HexDimension>();
        scriptedAI = this.gameObject.GetComponent<ScriptedAIBattleController>(); // can be null

        ////// DEBUG CODE //////
        if (battleMap == null) {
            Debug.Log("BattleMap needs to be set -> MapLoader.cs");
        }
        if (hexDimension == null) {
            Debug.Log("HexDimension needs to be set -> MapLoader.cs");
        }
        if (battleMap.selectionController == null) {
            Debug.Log("Major Error :: Hex Map Needs Selection Controller");
        }
        ////////////////////////
    }

    void Start() {
        if (battleMap != null && hexDimension != null) {
            LoadHexMap(csvMapFile);
        }
    }

    void LoadHexMap(string hexMapFile) {
        string[] mapCsvRows = GameResources.GetFileLines(mapsDir + hexMapFile);
        battleMap.ClearMap();
        int currentLine = 0;

        string[] dimensionRow = mapCsvRows[currentLine++].Split(',');
        if (dimensionRow.Length != 2) Debug.Log("ERROR :: INVALID FORMAT FOR LEVEL FILE");
        int rows = Convert.ToInt32(dimensionRow[0]);
        int cols = Convert.ToInt32(dimensionRow[1]);
        LoadMapTiles(mapCsvRows, rows);
        currentLine += rows;

        if(!isTutorial && currentLine < mapCsvRows.Length) {
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

        if (isTutorial)
        {
            AddUnitToTile(5, 5, scriptedAI.aiUnits[0], false, new Vector3(1, 0, 0));
            AddUnitToTile(10, 4, scriptedAI.aiUnits[1], false, new Vector3(0, 1, 0));
            AddUnitToTile(6, 6, scriptedAI.aiUnits[2], false, new Vector3(0, 1, 0));
            AddUnitToTile(4, 2, scriptedAI.aiUnits[3], true, new Vector3(2f, -1f, 0));
            AddUnitToTile(3, 3, scriptedAI.aiUnits[4], true, new Vector3(2f, -1f, 0));
        }
    }

    private void AddUnitToTile(int row, int col, Unit unit, bool isPlayerUnit, Vector3 facing)
    {
        unit.SetTile(HexMap.mapArray[row][col]);
        unit.SetFacing(facing);
        if (!isPlayerUnit)
        {
            unit.gameObject.AddComponent<BasicUnitAI>();
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
            tileObj.GetComponent<Tile>().selectionController = battleMap.selectionController;
            TileLocation location = tileObj.GetComponent<TileLocation>();
            location.col = col;
            location.row = row;
        }
        return tileObj;
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
            int move = Convert.ToInt32(data[8]);
            int direction = Convert.ToInt32(data[12]);
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
            stats.mvtRange = move;
            stats.className = type;
            ///////////////////////
            unit.SetTile(HexMap.mapArray[unitRow][unitCol]);
            unit.facing = direction;
            unitObject.AddComponent<BasicUnitAI>();
        }
        //Debug.Log("LoadedEnemies");
    }

    private void LoadDeploymentZone(string[] mapCvsLines, int startLineIndex, int numDep) {
        //Debug.Log("Number Of Deployment Zones :: " + numDep);
        DeploymentController deployController = FindObjectOfType(typeof(DeploymentController)) as DeploymentController;
        if (deployController != null && deployController.enabled) {
            for (int i = 0; i < numDep; i++) {
                string[] data = mapCvsLines[startLineIndex + i].Split(',');
                int depRow = Convert.ToInt32(data[0]);
                int depCol = Convert.ToInt32(data[1]);
                deployController.AddDeploymentTile(depRow, depCol);
            }
        }
    }
}

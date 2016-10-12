using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MapLoader : MonoBehaviour {
    private static readonly string mapsDir = "Maps/";
    public string csvMapFile;

    private HexMap battleMap;
    private HexDimension hexDimension;

    void Awake() {
        battleMap = this.gameObject.GetComponent<HexMap>();
        hexDimension = this.gameObject.GetComponent<HexDimension>();

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

    // row and colum indices start at 0 at upper left corner
    void Start() {
        if (battleMap != null && hexDimension != null) {
            LoadHexMap(csvMapFile);
            LoadPlayerUnits();
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

        if(currentLine < mapCsvRows.Length) {
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
            tileObj.GetComponent<Tile>().selectionController = battleMap.selectionController;
            TileLocation location = tileObj.GetComponent<TileLocation>();
            location.col = col;
            location.row = row;
        }
        return tileObj;
    }

    private void LoadEnemyUnits(string[] mapCvsLines, int startLineIndex, int numUnits) {
        Debug.Log("Number Of Units :: " + numUnits);
        for (int i = 0; i < numUnits; i++) {
            string[] data = mapCvsLines[startLineIndex + i].Split(',');
            int unitRow = Convert.ToInt32(data[0]);
            int unitCol = Convert.ToInt32(data[1]);
            string type = data[data.Length - 1].Trim();
            GameObject unitObject = null;
            unitObject = Instantiate(Resources.Load("Units/" + type)) as GameObject;
            Unit unit = unitObject.GetComponent<Unit>();
            unit.SetTile(HexMap.mapArray[unitRow][unitCol]);
            unit.SetFacing(new Vector3(0, 0, 0));
            unitObject.AddComponent<BasicUnitAI>();
        }
        Debug.Log("LoadedEnemies");
    }

    private void LoadDeploymentZone(string[] mapCvsLines, int startLineIndex, int numDep) {
        Debug.Log("Number Of Deployment Zones :: " + numDep);
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

    private void LoadPlayerUnits() {
        Vector2 faceRight = new Vector2(1, 0);
        for(int i = 0; i < 4; i++) {
            AddUnitToTile(i, 0, true, faceRight);
        }
    }

    private void AddUnitToTile(int row, int col, bool isPlayerUnit, Vector3 facing) {
        GameObject unitObject = null;
        if (isPlayerUnit)
            unitObject = Instantiate(Resources.Load("Units/Rifleman")) as GameObject;
        else
            unitObject = Instantiate(Resources.Load("Units/Swordsman")) as GameObject;
        Unit unit = unitObject.GetComponent<Unit>();
        unit.SetTile(HexMap.mapArray[row][col]);
        unit.SetFacing(facing);
        if(!isPlayerUnit) {
            unitObject.AddComponent<BasicUnitAI>();
        }
    }
}

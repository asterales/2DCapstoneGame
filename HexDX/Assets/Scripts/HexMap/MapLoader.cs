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
        if (battleMap == null)
        {
            Debug.Log("BattleMap needs to be set -> MapLoader.cs");
        }
        if (hexDimension == null)
        {
            Debug.Log("HexDimension needs to be set -> MapLoader.cs");
        }
        if (battleMap.selectionController == null)
        {
            Debug.Log("Major Error :: Hex Map Needs Selection Controller");
        }
        ////////////////////////
    }

    // row and colum indices start at 0 at upper left corner
    void Start() {
        if (battleMap != null && hexDimension != null) {
            // load the test battle map
            LoadHexMap(csvMapFile);
            LoadUnits();
        }
    }

    void LoadHexMap(string hexMapFile)
    {
        string[] mapCsvRows = GameResources.GetFileLines(mapsDir + hexMapFile);
        battleMap.ClearMap();

        string[] dimensionRow = mapCsvRows[0].Split(',');

        if (dimensionRow.Length != 2) Debug.Log("ERROR :: INVALID FORMAT FOR LEVEL FILE");

        int rows = Convert.ToInt32(dimensionRow[0]);
        int cols = Convert.ToInt32(dimensionRow[1]);

        List<Tile> row;
        float x = 0;
        float y = 0;
        float z = 0;
        int rowIndex = 0;

        for (int i = 0; i < rows; i++)
        {
            // Create new object for row in map, make it a subobject of hexMap
            // row objects are useless other than for organizational purposes so putting DEBUG around them
            ////// DEBUG CODE //////
            GameObject rowObj = CreateNewRowObj(rowIndex);
            ////////////////////////
            string csvRow = mapCsvRows[i + 1];

            row = new List<Tile>();
            int columnIndex = 0;
            string[] line = csvRow.Split(',');

            foreach (string num in line)
            {
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
            rowIndex++;
        }

        int numUnits = Convert.ToInt32(mapCsvRows[++rowIndex]);
        Debug.Log("Number Of Units :: " + numUnits);
        // Unit Parsing
        for (int i = 0; i < numUnits; i++)
        {
            string[] data = mapCsvRows[++rowIndex].Split(',');
            int unitRow = Convert.ToInt32(data[0]);
            int unitCol = Convert.ToInt32(data[1]);
            string type = data[data.Length - 1].Trim();
            GameObject unitObject = null;
            Debug.Log(type);
            Debug.Log("Units/" + type);
            unitObject = Instantiate(Resources.Load("Units/" + type)) as GameObject;
            Unit unit = unitObject.GetComponent<Unit>();
            unit.SetTile(HexMap.mapArray[unitRow][unitCol]);
            unit.SetFacing(new Vector3(0, 0, 0));
            unitObject.AddComponent<BasicUnitAI>();
        }

        int numDep = Convert.ToInt32(mapCsvRows[++rowIndex]);
        Debug.Log("Number Of Deployment Zones :: " + numDep);
        // DeploymentZone parsing
        for (int i = 0; i < numDep; i++)
        {
            string[] data = mapCsvRows[++rowIndex].Split(',');
            int depRow = Convert.ToInt32(data[0]);
            int depCol = Convert.ToInt32(data[1]);
            CreateDeploymentZoneTile(depRow, depCol);
        }
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

    private void CreateDeploymentZoneTile(int row, int col)
    {
        // to be implemented
    }

    // Debug/organizational purposes - can be removed later
    private GameObject CreateNewRowObj(int rowIndex) {
        GameObject rowObj = new GameObject("Row " + rowIndex);
        rowObj.transform.parent = battleMap.transform;
        rowObj.transform.localPosition = new Vector3(0, 0, 0);
        return rowObj;
    }

    private void LoadUnits() {
        Vector2 faceRight = new Vector2(1, 0);
        //Vector2 faceLeft = new Vector2(-1, 0);
        for(int i = 0; i < 4; i++) {
            AddUnitToTile(i, 0, true, faceRight);
            //AddUnitToTile(i, 5, false, faceLeft);
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

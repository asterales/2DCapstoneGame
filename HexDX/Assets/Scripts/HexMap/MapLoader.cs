﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MapLoader : MonoBehaviour {
    public List<Sprite> sprites;
    public string csvMapFile = "Assets/Maps/test.csv";

    private HexMap battleMap;
    private HexDimension hexDimension;
    private BattleController battleController;

    void Awake() {
        battleMap = this.gameObject.GetComponent<HexMap>();
        hexDimension = this.gameObject.GetComponent<HexDimension>();
        battleController = this.gameObject.GetComponent<BattleController>();

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

    void LoadHexMap(string hexMapFile) {
        var reader = new StreamReader(File.OpenRead(hexMapFile));
        battleMap.ClearMap();

        List<Tile> row;
        float x = 0;
        float y = 0;
        float z = 0;
        int rowIndex = 0;

        while (!reader.EndOfStream) {
            // Create new object for row in map, make it a subobject of hexMap
            // row objects are useless other than for organizational purposes so putting DEBUG around them
            ////// DEBUG CODE //////
            GameObject rowObj = CreateNewRowObj(rowIndex);
            ////////////////////////

            row = new List<Tile>();
            int columnIndex = 0;
            string[] line = reader.ReadLine().Split(',');

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
            rowIndex++;
        }
    }

    private GameObject CreateTile(int type, Vector3 pos, GameObject rowObj, int row, int col) {
        GameObject tileObj = InstantiateTileFromPrefab(type);
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

    private GameObject InstantiateTileFromPrefab(int type) {
        switch(type) {
            case 0:
                return Instantiate(Resources.Load("Tiles/GrassTile")) as GameObject;
            case 1:
                return Instantiate(Resources.Load("Tiles/MountainTile")) as GameObject;
            default:
                return null;
        }
    }

    // Debug/organizational purposes - can be removed later
    private GameObject CreateNewRowObj(int rowIndex) {
        GameObject rowObj = new GameObject("Row " + rowIndex);
        rowObj.transform.parent = battleMap.transform;
        rowObj.transform.localPosition = new Vector3(0, 0, 0);
        return rowObj;
    }

    private void LoadUnits() {
        GameObject knight = null;
        Tile tile = HexMap.mapArray[0][0];
        knight = Instantiate(Resources.Load("Units/Swordsman")) as GameObject;
        knight.transform.parent = tile.gameObject.transform;
        knight.transform.position = tile.gameObject.transform.position;
        Unit unit = knight.GetComponent<Unit>();
        tile.currentUnit = unit;
        unit.currentTile = tile;
        unit.isPlayerUnit = true;
        
        tile = HexMap.mapArray[11][15];
        knight = Instantiate(Resources.Load("Units/Swordsman")) as GameObject;
        knight.transform.parent = tile.gameObject.transform;
        knight.transform.position = tile.gameObject.transform.position;
        unit = knight.GetComponent<Unit>();
        tile.currentUnit = unit;
        unit.currentTile = tile;
        unit.isPlayerUnit = false;

        tile = HexMap.mapArray[10][5];
        knight = Instantiate(Resources.Load("Units/Swordsman")) as GameObject;
        knight.transform.parent = tile.gameObject.transform;
        knight.transform.position = tile.gameObject.transform.position;
        unit = knight.GetComponent<Unit>();
        tile.currentUnit = unit;
        unit.currentTile = tile;
        unit.isPlayerUnit = false;
    }
}

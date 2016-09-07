using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MapLoader : MonoBehaviour {
    public List<Sprite> sprites;
    public string csvMapFile = "Assets/Maps/test.csv";

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
        if (battleMap != null && hexDimension != null)
        {
            // load the test battle map
            LoadHexMap(csvMapFile);
        }
    }

    void LoadHexMap(string hexMapFile)
    {
        var reader = new StreamReader(File.OpenRead(hexMapFile));
        battleMap.ClearMap();

        List<Tile> row;
        float x = 0;
        float y = 0;
        int rowIndex = 0;

        while (!reader.EndOfStream)
        {
            // Create new object for row in map, make it a subobject of hexMap
            // row objects are useless other than for organizational purposes so putting DEBUG around them
            ////// DEBUG CODE //////
            GameObject rowObj = CreateNewRowObj(rowIndex);
            ////////////////////////

            row = new List<Tile>();
            int columnIndex = 0;
            string[] line = reader.ReadLine().Split(',');

            foreach (string num in line)
            {
                GameObject tileObj = CreateTileFromPrefab(int.Parse(num),new Vector3(x, y, 0), rowObj, rowIndex, columnIndex);
                Tile tile = tileObj.GetComponent<Tile>();
                tile.selectionController = battleMap.selectionController;
                row.Add(tile);
                x += 2 * hexDimension.width;
                columnIndex++;
            }

            HexMap.mapArray.Add(row);
            y -= 2*hexDimension.apex-hexDimension.minorApex;
            x -= 2 * hexDimension.width * line.Length + hexDimension.width;
            rowIndex++;
        }
    }

    private GameObject CreateTileFromPrefab(int type, Vector3 pos, GameObject rowObj, int row, int col)
    {
        GameObject tileObj = null;
        GameObject knight = null;
        switch (type)
        {
            case 0:
                tileObj = Instantiate(Resources.Load("Tiles/GrassTile")) as GameObject;
                break;
        }
        tileObj.name = string.Format("Tile ({0}, {1})", row, col);
        tileObj.transform.parent = rowObj.transform;
        tileObj.transform.localPosition = pos;
        Tile tile = tileObj.GetComponent<Tile>();
        tile.movementTile = Instantiate(Resources.Load("Tiles/MovementTile")) as GameObject;
        tile.movementTile.transform.position = tileObj.transform.position + new Vector3(0.0f, 0.0f, 0.1f) ;
        if (row ==0 && col==0)
        {
            knight = Instantiate(Resources.Load("Units/Swordsman")) as GameObject;
            knight.transform.parent = tileObj.transform;
            tile.currentUnit = knight.GetComponent<Unit>();
            tile.currentUnit.GetComponent<Unit>().currentTile = tile;
            knight.transform.position = tileObj.transform.position;
        }
        TileLocation location = tileObj.GetComponent<TileLocation>();
        location.col = col;
        location.row = row;
        return tileObj;
    }

    // Debug/organizational purposes - can be removed later
    private GameObject CreateNewRowObj(int rowIndex){
        GameObject rowObj = new GameObject("Row " + rowIndex);
        rowObj.transform.parent = battleMap.transform;
        rowObj.transform.localPosition = new Vector3(0, 0, 0);
        return rowObj;
    }
}

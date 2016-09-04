using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MapLoader : MonoBehaviour {
    public List<Sprite> sprites;
    public string csvMapFile = "Assets/Maps/test.csv";

    private HexMap battleMap;
    private HexDimension hexDimension;

    // row and colum indices start at 0 at upper left corner
    void Start () {
        battleMap = this.gameObject.GetComponent<HexMap>();
        hexDimension = this.gameObject.GetComponent<HexDimension>();

        ////// DEBUG CODE //////
        bool valid = true;
        if (battleMap == null)
        {
            Debug.Log("BattleMap needs to be set -> MapLoader.cs");
            valid = false;
        }
        if (hexDimension == null)
        {
            Debug.Log("HexDimension needs to be set -> MapLoader.cs");
            valid = false;
        }
        ////////////////////////

        if (valid)
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
        Sprite sprite = null;
        int rowIndex = 0;

        while (!reader.EndOfStream)
        {
            // Create new object for row in map, make it a subobject of hexMap
            // row objects are useless other than for organizational purposes so putting DEBUG around them
            ////// DEBUG CODE //////
            GameObject rowObj = new GameObject("Row " + rowIndex);
            rowObj.transform.parent = battleMap.transform;
            rowObj.transform.localPosition = new Vector3(0, 0, 0);
            ////////////////////////

            row = new List<Tile>();
            int columnIndex = 0;
            string[] line = reader.ReadLine().Split(',');

            foreach (string num in line)
            {
                // replace this with a prefab in the future -> use CreateTileFromPrefab
                // <--
                GameObject tileObj = new GameObject(string.Format("Tile ({0}, {1})", rowIndex, columnIndex));
                tileObj.transform.parent = rowObj.transform;
                tileObj.transform.localPosition = new Vector3(x, y, 0);

                Tile tile = tileObj.AddComponent<Tile>();
                tileObj.AddComponent<SpriteRenderer>();
                sprite = sprites[int.Parse(num)];
                tile.SetTile((TileType)int.Parse(num), sprite);
                // -->

                row.Add(tile);
                x += 2 * sprite.bounds.extents.x;
                columnIndex++;
            }

            battleMap.mapArray.Add(row);
            y -= 1.5f * sprite.bounds.extents.y;
            x -= 2 * sprite.bounds.extents.x * line.Length + sprite.bounds.extents.x;
            rowIndex++;
        }
    }

    private GameObject CreateTileFromPrefab(int type)
    {
        Debug.Log("TO BE IMPLEMENTED");
        return null;
    }

    private GameObject CreateHexMapObj(Vector3 upperLeftPos){
        GameObject mapObj = new GameObject("HexMap");
        mapObj.AddComponent<HexMap>();
        mapObj.AddComponent<HexDimension>();
        mapObj.transform.position = upperLeftPos;
        return mapObj;
    }

    private GameObject CreateewRowObj(Vector3 startPos, GameObject mapObj, int rowIndex){
        GameObject rowObj = new GameObject("Row " + rowIndex);
        rowObj.transform.position = startPos;
        rowObj.transform.parent = mapObj.transform;
        return rowObj;
    }

    private GameObject CreateNewTileObj(Vector3 pos, GameObject rowObj, int row, int col){
        GameObject tileObj = new GameObject(string.Format("Tile ({0}, {1})", row, col));
        tileObj.AddComponent<Tile>();
        tileObj.AddComponent<SpriteRenderer>();
        tileObj.AddComponent<TileStats>();
        tileObj.transform.position = pos;
        tileObj.transform.parent = rowObj.transform;
        return tileObj;
    }
}

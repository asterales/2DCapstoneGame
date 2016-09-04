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
        Sprite sprite = null;
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
                // replace this with a prefab in the future -> use CreateTileFromPrefab
                // <--
                GameObject tileObj = CreateNewTileObj(new Vector3(x, y, 0), rowObj, rowIndex, columnIndex);
                Tile tile = tileObj.AddComponent<Tile>();
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

    // Debug/organizational purposes - can be removed later
    private GameObject CreateNewRowObj(int rowIndex){
        GameObject rowObj = new GameObject("Row " + rowIndex);
        rowObj.transform.localPosition = new Vector3(0, 0, 0);
        rowObj.transform.parent = battleMap.transform;
        return rowObj;
    }

    // Remove later
    private GameObject CreateNewTileObj(Vector3 pos, GameObject rowObj, int row, int col){
        GameObject tileObj = new GameObject(string.Format("Tile ({0}, {1})", row, col));
        tileObj.AddComponent<Tile>();
        tileObj.AddComponent<SpriteRenderer>();
        tileObj.AddComponent<TileStats>();
        tileObj.transform.localPosition = pos;
        tileObj.transform.parent = rowObj.transform;
        return tileObj;
    }
}

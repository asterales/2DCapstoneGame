using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MapLoader : MonoBehaviour {
    public List<Sprite> sprites;
    public string csvMapFile = "Assets/Maps/test.csv";

    // Use this for initialization
    // row and colum indices start at 0 at upper left corner
    void Start () {
        var reader = new StreamReader(File.OpenRead(csvMapFile));
        List<List<Tile>> map = new List<List<Tile>>();
        List<Tile> row;
        float x = -5;
        float y = 5;
        Sprite sprite = null;
        GameObject mapObj = GetHexMapObj(new Vector3(x, y, 0));
        int rowIndex = 0;
        while (!reader.EndOfStream)
        {
            GameObject rowObj = GetNewRowObj(new Vector3(x, y, 0), mapObj, rowIndex);
            row = new List<Tile>();
            int columnIndex = 0;
            string[] line = reader.ReadLine().Split(',');
            foreach (string num in line) {
                GameObject tileObj = GetNewTileObj(new Vector3(x, y, 0), rowObj, rowIndex, columnIndex);
                Tile tile = tileObj.GetComponent<Tile>();
                sprite = sprites[int.Parse(num)];
                tile.SetTile((TileType)int.Parse(num),sprite);
                row.Add(tile);
                x += 2*sprite.bounds.extents.x;
                columnIndex++;
            }
            map.Add(row);
            y -= 1.5f*sprite.bounds.extents.y;
            x -= 2 * sprite.bounds.extents.x*line.Length+sprite.bounds.extents.x;
            rowIndex++;
        }
        // Set the tiles to keep track of in the HexMap script
        mapObj.GetComponent<HexMap>().SetMap(map);
    }

    private GameObject GetHexMapObj(Vector3 upperLeftPos){
        GameObject mapObj = new GameObject("HexMap");
        mapObj.AddComponent<HexMap>();
        mapObj.AddComponent<HexDimension>();
        mapObj.transform.position = upperLeftPos;
        return mapObj;
    }

    private GameObject GetNewRowObj(Vector3 startPos, GameObject mapObj, int rowIndex){
        GameObject rowObj = new GameObject("Row " + rowIndex);
        rowObj.transform.position = startPos;
        rowObj.transform.parent = mapObj.transform;
        return rowObj;
    }

    private GameObject GetNewTileObj(Vector3 pos, GameObject rowObj, int row, int col){
        GameObject tileObj = new GameObject(string.Format("Tile ({0}, {1})", row, col));
        tileObj.AddComponent<Tile>();
        tileObj.AddComponent<SpriteRenderer>();
        tileObj.AddComponent<TileStats>();
        tileObj.transform.position = pos;
        tileObj.transform.parent = rowObj.transform;
        return tileObj;
    }
}

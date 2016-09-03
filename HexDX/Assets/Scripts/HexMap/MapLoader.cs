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
        // Create new object for map and attach a new HexMap script to it
        GameObject mapObj = new GameObject("HexMap");
        HexMap hexMap = mapObj.AddComponent<HexMap>();
        mapObj.transform.position = new Vector3(x, y, 0);
        int rowIndex = 0;
        while (!reader.EndOfStream)
        {
            //Create new object for row in map, make it a subobject of mapObj
            GameObject rowObj = new GameObject("Row " + rowIndex);
            rowObj.transform.position = new Vector3(x, y, 0);
            rowObj.transform.parent = mapObj.transform;
            row = new List<Tile>();
            int columnIndex = 0;
            string[] line = reader.ReadLine().Split(',');
            foreach (string num in line) {
                GameObject tileObj = new GameObject(string.Format("Tile ({0}, {1})", rowIndex, columnIndex));
                tileObj.transform.position = new Vector3(x,y,0);
                tileObj.transform.parent = rowObj.transform;
                Tile tile = tileObj.AddComponent<Tile>();
                tileObj.AddComponent<SpriteRenderer>();
                sprite = sprites[int.Parse(num)];
                tile.MakeTile(int.Parse(num),sprite);
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
        hexMap.SetMap(map);
    }
}

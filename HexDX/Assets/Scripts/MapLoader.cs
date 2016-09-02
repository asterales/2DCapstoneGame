using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MapLoader : MonoBehaviour {
    public List<List<Tile>> map = new List<List<Tile>>();
    public List<Sprite> sprites;

    // Use this for initialization
    void Start () {
        var reader = new StreamReader(File.OpenRead("Assets/Maps/test.csv"));
        List<Tile> row;
        float x = -5;
        float y = 5;
        Sprite sprite = null;
        while (!reader.EndOfStream)
        {
            string[] line = reader.ReadLine().Split(',');
            row = new List<Tile>();
            foreach (string num in line) {
                GameObject tile = new GameObject();
                Tile t = tile.AddComponent<Tile>();
                tile.AddComponent<SpriteRenderer>();
                sprite = sprites[int.Parse(num)];
                t.MakeTile(int.Parse(num),sprite);
                tile.transform.position = new Vector3(x,y,0);
                row.Add(t);
                x += 2*sprite.bounds.extents.x;
            }
            map.Add(row);
            y -= 1.5f*sprite.bounds.extents.y;
            x -= 2 * sprite.bounds.extents.x*line.Length+sprite.bounds.extents.x;

        }


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

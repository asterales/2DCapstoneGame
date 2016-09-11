using UnityEngine;
using System.Collections.Generic;

public class LEHexMap : MonoBehaviour {
    public LESelectionController selectionController;
    private HexDimension hexDimension;
    public LESpriteCache spriteCache;
    public List<List<LETile>> tileArray;

    void Awake()
    {
        hexDimension = this.gameObject.GetComponent<HexDimension>();
        spriteCache = this.gameObject.GetComponent<LESpriteCache>();
        selectionController = this.gameObject.GetComponent<LESelectionController>();
        tileArray = new List<List<LETile>>();

        ////// DEBUG CODE //////
        if (hexDimension == null)
        {
            Debug.Log("HexDimension needs to be defined -> LEHexMap.cs");
        }
        if (spriteCache == null)
        {
            Debug.Log("Sprite Cache needs to be set -> LEHexMap.cs");
        }
        this.gameObject.transform.position = new Vector3(hexDimension.globalTopLeftX, hexDimension.globalTopLeftY, 0); // temp
        ////////////////////////

        // create the tile array
        // this is a temporary size arbitrarily chosen
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        for (int i=0;i<12;i++)
        {
            tileArray.Add(new List<LETile>());
            for (int j=0;j<16;j++)
            {
                // TODO :: Replace with a prefab later
                Vector3 pos = new Vector3(x, y, z); // to be changed
                GameObject newTile = CreateTileObject(pos, i, j);
                tileArray[i].Add(newTile.GetComponent<LETile>());
                x += 2 * hexDimension.width;
            }
            y -= 2 * hexDimension.apex - hexDimension.minorApex;
            x -= 2 * hexDimension.width * 16 - hexDimension.width;
            z -= .001f;
        }
    }

    public GameObject CreateTileObject(Vector3 pos, int row, int col)
    {
        GameObject newTile = new GameObject(string.Format("Tile ({0}, {1})", row, col));
        newTile.transform.parent = this.gameObject.transform;
        newTile.AddComponent<SpriteRenderer>();
        newTile.AddComponent<TileLocation>();
        newTile.AddComponent<BoxCollider2D>();
        newTile.AddComponent<LETile>();
        newTile.transform.localPosition = pos;
        // tile location in the map
        TileLocation location = newTile.GetComponent<TileLocation>();
        location.col = col;
        location.row = row;
        // box collider initial settings (temp values)
        newTile.GetComponent<BoxCollider2D>().size = new Vector2(8f, 5f);
        newTile.GetComponent<LETile>().spriteCache = spriteCache;
        newTile.GetComponent<LETile>().ChangeType(0);
        newTile.GetComponent<LETile>().reference = this;
        return newTile;
    }

    public void ClearMap()
    {
        // sets all tiles to the default type
        for (int i = 0; i < tileArray.Count; i++)
        {
            for (int j = 0; j < tileArray[i].Count; j++)
            {
                tileArray[i][j].ChangeType(0);
            }
        }
    }
}

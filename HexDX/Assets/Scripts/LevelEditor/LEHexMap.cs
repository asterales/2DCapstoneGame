using UnityEngine;
using System.Collections.Generic;

public class LEHexMap : MonoBehaviour {
    public LESelectionController selectionController;
    public HexDimension hexDimension;
    public LESpriteCache spriteCache;
    public LEUnitCache unitCache;
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
        if (unitCache == null)
        {
            Debug.Log("Unit Cache needs to be set -> LEHexMap.cs");
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
        newTile.GetComponent<LETile>().unitCache = unitCache;
        newTile.GetComponent<LETile>().ChangeType(0);
        newTile.GetComponent<LETile>().reference = this;
        return newTile;
    }

    public void CleanMap()
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

    public void ClearMap()
    {
        // remove and delete all current tiles
        for (int i = 0; i < tileArray.Count; i++)
        {
            while (tileArray[i].Count > 0)
            {
                int lastIndex = tileArray[i].Count - 1;
                LETile tile = tileArray[i][lastIndex];
                tileArray[i].RemoveAt(lastIndex);
                // maybe do more cleanup or caching here
                Destroy(tile);
                Destroy(tile.gameObject);
            }
        }

        tileArray = new List<List<LETile>>();
    }

    public void AppendRow()
    {
        tileArray.Add(new List<LETile>());
    }

    public void AppendTile(GameObject obj)
    {
        tileArray[tileArray.Count - 1].Add(obj.GetComponent<LETile>());
    }

    public void IncrementVert()
    {
        //Debug.Log("Incrementing Vert Dim");
        int size = tileArray[0].Count;
        int row = tileArray.Count;
        List<LETile> newRow = new List<LETile>();

        float x = tileArray[row-1][size-1].gameObject.transform.localPosition.x;
        float y = tileArray[row-1][size-1].gameObject.transform.localPosition.y;
        float z = tileArray[row-1][size-1].gameObject.transform.localPosition.z;
        x -= 2 * hexDimension.width * (size-1) - hexDimension.width;
        y -= 2 * hexDimension.apex - hexDimension.minorApex;
        z -= .001f;

        for (int i = 0; i < size; i++)
        {
            Vector3 pos = new Vector3(x, y, z);
            x += 2 * hexDimension.width;
            GameObject newTileObject = CreateTileObject(pos, row, i);
            LETile newTile = newTileObject.GetComponent<LETile>();
            newRow.Add(newTile);
        }
        tileArray.Add(newRow);
    }

    public void IncrementHori()
    {
        //Debug.Log("Incrementing Hori Dim");
        for(int i=0;i<tileArray.Count;i++)
        {
            List<LETile> tileRow = tileArray[i];
            float x = tileRow[tileRow.Count - 1].gameObject.transform.localPosition.x;
            float y = tileRow[tileRow.Count - 1].gameObject.transform.localPosition.y;
            float z = tileRow[tileRow.Count - 1].gameObject.transform.localPosition.z;
            x += 2 * hexDimension.width;
            Vector3 pos = new Vector3(x, y, z);
            GameObject newTileObject = CreateTileObject(pos, i, tileRow.Count);
            LETile newTile = newTileObject.GetComponent<LETile>();
            tileRow.Add(newTile);
        }
    }

    public void DecrementHori()
    {
        //Debug.Log("Decremening Hori Dim");
        if(tileArray[0].Count > 1)
        {
            for(int i=0;i<tileArray.Count;i++)
            {
                List<LETile> row = tileArray[i];
                LETile lastTile = row[row.Count - 1];
                row.RemoveAt(row.Count - 1);
                lastTile.PrepareToDestroy();
                Destroy(lastTile.gameObject);
            }
        }
    }

    public void DecrementVert()
    {
        //Debug.Log("Decrementing Vert Dim");
        if (tileArray.Count > 1)
        {
            List<LETile> lastArray = tileArray[tileArray.Count - 1];
            for (int i = 0; i < lastArray.Count; i++)
            {
                lastArray[i].PrepareToDestroy();
                Destroy(lastArray[i].gameObject);
            }
            tileArray.RemoveAt(tileArray.Count - 1);
        }
    }
}

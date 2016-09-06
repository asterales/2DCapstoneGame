﻿using UnityEngine;
using System.Collections.Generic;

public class LEHexMap : MonoBehaviour {
    private HexDimension hexDimension;
    public LESpriteCache spriteCache;
    public List<List<LETile>> tileArray;

    void Awake()
    {
        hexDimension = this.gameObject.GetComponent<HexDimension>();
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
        for (int i=0;i<12;i++)
        {
            tileArray.Add(new List<LETile>());
            for (int j=0;j<16;j++)
            {
                // TODO :: Replace with a prefab later
                Vector3 pos = new Vector3(0, 0, 0); // to be changed
                GameObject newTile = CreateTileObject(pos, i, j);
                tileArray[i].Add(newTile.GetComponent<LETile>());
            }
        }
    }

    public GameObject CreateTileObject(Vector3 pos, int row, int col)
    {
        GameObject newTile = new GameObject(string.Format("Tile ({0}, {1})", row, col));
        newTile.AddComponent<SpriteRenderer>();
        newTile.AddComponent<TileLocation>();
        newTile.AddComponent<BoxCollider2D>();
        newTile.AddComponent<LETile>();
        newTile.transform.localPosition = pos;
        // tile location in the map
        TileLocation location = newTile.GetComponent<TileLocation>();
        location.xpos = col;
        location.ypos = row;
        // box collider initial settings (temp values)
        newTile.GetComponent<BoxCollider2D>().size = new Vector2(2.22f, 1.03f);
        newTile.GetComponent<LETile>().spriteCache = spriteCache;
        newTile.GetComponent<LETile>().ChangeType(0);
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

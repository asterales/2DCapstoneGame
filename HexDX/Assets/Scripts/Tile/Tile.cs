﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
    public int type; // made public for testing purposes
    public bool isPathable;
    private TileStats tileStats;

    public void Start() {
        tileStats = this.gameObject.GetComponent<TileStats>();
        ////// DEBUG CODE //////
        if (tileStats == null)
        {
            Debug.Log("Error :: Object Must Have TileStats Object -> Tile.cs");
        }
        ////////////////////////
    }

    public void MakeTile(int type, Sprite sprite)
    {
        this.type = type;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}

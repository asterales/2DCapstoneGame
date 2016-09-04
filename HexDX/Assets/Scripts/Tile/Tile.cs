using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
    public TileType type; // made public for testing purposes
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

    public void SetTile(TileType type, Sprite sprite)
    {
        this.type = type;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public bool IsPathable(){
        return type.IsPathable(); // && not occupied by unit?
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
    public TileType type; // made public for testing purposes
    public TileLocation position;
    public SelectionController selectionController; // hack for movement
    private TileStats tileStats;

    public void Awake () {
        tileStats = this.gameObject.GetComponent<TileStats>();
        position = this.gameObject.GetComponent<TileLocation>();
        ////// DEBUG CODE //////
        if (tileStats == null)
        {
            Debug.Log("Error :: Object Must Have TileStats Object -> Tile.cs");
        }

        if (position == null)
        {
            Debug.Log("Error :: Object Must Have TileLocation Object -> Tile.cs");
        }
        ////////////////////////
    }

    public void SetTile(TileType type, Sprite sprite, SelectionController sc)
    {
        this.type = type;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        selectionController = sc;
    }

    public bool IsPathable(){
        return type.IsPathable(); // && not occupied by unit?
    }

    public void OnMouseDown()
    {
        selectionController.clickedTile = this;
    }
}

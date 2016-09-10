using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
    public bool pathable;
    public TileLocation position;
    public SelectionController selectionController; // hack for movement
    public Unit currentUnit;
    private TileStats tileStats;
    private GameObject movementTile;
    private static readonly Vector3 visibilityOffset = new Vector3(0, 0, -0.01f);

    public void Awake() {
        tileStats = this.gameObject.GetComponent<TileStats>();
        position = this.gameObject.GetComponent<TileLocation>();
        InitMovementTile();
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

    private void InitMovementTile() {
        if (pathable) {
            movementTile = Instantiate(Resources.Load("Tiles/MovementTile")) as GameObject;
            movementTile.transform.parent = transform;
            movementTile.GetComponent<MovementTile>().tile = this;
            HideMovementTile();
        }
    }

    public void Update() { }

    public void OnMouseDown() {
        HexMap.ClearMovementTiles();
        SelectionController.selectedTile = this;
        if (currentUnit) {
            HexMap.ShowMovementTiles(this, currentUnit.unitStats.mvtRange + 1);
        }
    }

    public void ShowMovementTile() {
        if (movementTile) {
            movementTile.transform.localPosition = visibilityOffset;
        }
    }

    public void HideMovementTile() {
        if (movementTile) {
            movementTile.transform.localPosition = -visibilityOffset;
        }
    }

    public bool MovementTileIsVisible() {
        return movementTile && (movementTile.transform.localPosition == visibilityOffset);
    }
}

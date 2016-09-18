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
    private GameObject attackTile;
    private static readonly Vector3 visibilityOffset = new Vector3(0, 0, -0.01f);

    public void Awake() {
        tileStats = this.gameObject.GetComponent<TileStats>();
        position = this.gameObject.GetComponent<TileLocation>();
        InitMovementTile();
        InitAttackTile();
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

    private void InitAttackTile() {
        // to be finished
        if (pathable) {
            attackTile = Instantiate(Resources.Load("Tiles/AttackTile")) as GameObject;
            attackTile.transform.parent = transform;
            attackTile.GetComponent<AttackTile>().tile = this;
            HideAttackTile();
        }
    }

    public void OnMouseOver() {
        if (SelectionController.TakingInput() && (Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1))) {
            //left click - selection
            HexMap.ClearAllTiles();
            MovementTile.path = null;
            SelectionController.selectedTile = this;
            SelectionController.selectedUnit = currentUnit ? currentUnit : SelectionController.selectedUnit;
            if (currentUnit){ 
                //TO ADD: display stats
                if (currentUnit.IsPlayerUnit()) {
                    if (currentUnit.phase == UnitTurn.Open){
                        HexMap.ShowMovementTiles(this, currentUnit.unitStats.mvtRange + 1);
                        MovementTile.path = new List<Tile>() { this };
                    }
                } else {
                    // show enemy mvt range and stats
                    HexMap.ShowMovementTiles(this, currentUnit.unitStats.mvtRange + 1);
                    HexMap.ShowAttackTiles(this);
                }
            }
        }
    }

    // was preventing player from selecting the option for the unit to not move from its spot during movement phase
    /*public void OnMouseUp() {
        if (SelectionController.TakingInput() && MovementTileIsVisible()) {
            MovementTile.CommitPath();
        }
    } */

    public void ShowMovementTile() {
        if (movementTile) {
            movementTile.transform.localPosition = visibilityOffset;
            HexMap.showingMovementTiles.Push(this);
            HideAttackTile();
        }
    }

    public void HideMovementTile() {
        if (movementTile) {
            movementTile.transform.localPosition = -visibilityOffset;
        }
    }
    public void ShowAttackTile() {
        if (attackTile) {
            attackTile.transform.localPosition = visibilityOffset;
            HexMap.showingAttackTiles.Push(this);
            HideMovementTile();
        }
    }
    public void HideAttackTile() {
        if (attackTile) {
            attackTile.transform.localPosition = -visibilityOffset;
        }
    }

    public bool MovementTileIsVisible() {
        return movementTile && (movementTile.transform.localPosition == visibilityOffset);
    }
}

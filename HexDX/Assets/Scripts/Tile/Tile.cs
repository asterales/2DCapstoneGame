﻿using UnityEngine;
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

    private void InitAttackTile()
    {
        // to be finished
        //if (pathable)
        //{
        //    attackTile = Instantiate(Resources.Load("Tiles/AttackTile")) as GameObject;
        //    attackTile.transform.parent = transform;
        //    attackTile.GetComponent<AttackTile>().tile = this;
        //    HideAttackTile();
        //}
    }

    public void Update() { }

    public void OnMouseDown() {
        if (SelectionController.TakingInput())
        {
            HexMap.ClearMovementTiles();
            SelectionController.selectedTile = this;
            if (currentUnit && currentUnit.phase != UnitTurn.Done)
            {
                Debug.Log("WHAt");
                HexMap.ShowMovementTiles(this, currentUnit.unitStats.mvtRange + 1);
                MovementTile.path = new List<Tile>() { this };
            }
        }
    }

    public void OnMouseUp()
    {
        if (SelectionController.TakingInput())
        {
            if (MovementTile.path != null && MovementTile.path.Count > 1)
            {
                SelectionController.selectedUnit.path = new Queue<Tile>(MovementTile.path);
                MovementTile.path = null;
                HexMap.ClearMovementTiles();
                SelectionController.ClearSelection();
            }
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

    public void HideAttackTile()
    {
        if (attackTile)
        {
            attackTile.transform.localPosition = -visibilityOffset;
        }
    }

    public bool MovementTileIsVisible() {
        return movementTile && (movementTile.transform.localPosition == visibilityOffset);
    }
}

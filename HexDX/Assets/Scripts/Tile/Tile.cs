﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
    public bool pathable;
    public TileLocation position;
    public SelectionController selectionController; // hack for movement
    public Unit currentUnit;
    public TileStats tileStats;
    private GameObject movementTile;
    private GameObject attackTile;
    private static readonly Vector3 visibilityOffset = new Vector3(0, 0, -0.01f);

    public void Awake() {
        tileStats = this.gameObject.GetComponent<TileStats>();
        position = this.gameObject.GetComponent<TileLocation>();
        InitMovementTile();
        InitAttackTile();
        InitOutline();
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

    private void InitOutline() {
        GameObject outline = (GameObject)GameObject.Instantiate(Resources.Load("Tiles/Outline"));
        outline.transform.parent = this.transform;
        outline.transform.localPosition = Vector3.zero;
    }

    public void OnMouseOver() {
        if ((SelectionController.TakingInput() || TutorialController.IsTargetTile(this))
                && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))) {
            //left click - selection
            HexMap.ClearAllTiles();
            MovementTile.path = null;
            SelectionController.selectedTile = this;
            SelectionController.selectedUnit = currentUnit ? currentUnit : SelectionController.selectedUnit;
            if (currentUnit) {
                //TO ADD: display stats
                if (currentUnit.IsPlayerUnit()) {
                    StatDisplay.DisplayPlayerUnit(currentUnit);
                    if (currentUnit.phase == UnitTurn.Open) {
                        HexMap.ShowMovementTiles(currentUnit);
                        MovementTile.path = new List<Tile>() { this };
                    }
                } else {
                    // show enemy mvt range and stats
                    StatDisplay.DisplayEnemyUnit(currentUnit);
                    List<Tile> movementTiles = HexMap.GetMovementTiles(currentUnit);
                    int facing = currentUnit.facing;
                    foreach (Tile t in movementTiles)
                    {
                        currentUnit.currentTile = t;
                        for (int i = 0; i < 6; i++)
                        {
                            currentUnit.facing = i;
                            foreach (Tile tile in HexMap.GetAttackTiles(currentUnit))
                                tile.ShowAttackTile();
                        }
                    }
                    currentUnit.facing = facing;
                    currentUnit.currentTile = this;
                    HexMap.ShowMovementTiles(currentUnit);
                    foreach (Tile tile in HexMap.GetAttackTiles(currentUnit))
                    {
                        tile.attackTile.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 0.95f);
                        tile.ShowAttackTile();
                    }
                }
            }
        }
    }

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
        attackTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.8f);
        if (attackTile) {
            attackTile.transform.localPosition = -visibilityOffset;
        }
    }
}

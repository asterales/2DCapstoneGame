using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
    public bool pathable;
    public TileLocation position;
    public Unit currentUnit;
    public TileStats tileStats;
    public List<RuntimeAnimatorController> animations;
    private GameObject movementTile;
    private GameObject attackTile;

    public void Awake() {
        tileStats = GetComponent<TileStats>();
        position = GetComponent<TileLocation>();
        InitMovementTile();
        InitAttackTile();
        //InitOutline();
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
        if (SelectionController.selectedUnit && SelectionController.selectedUnit.phase !=UnitTurn.Attacking)
            SelectionController.HideTarget();
        if (SelectionController.TakingInput() || (TutorialController.IsTargetTile(this)&& Input.GetMouseButtonDown(0))){
            if (currentUnit && !currentUnit.IsPlayerUnit()){
                if (SelectionController.selectedUnit && SelectionController.selectedUnit.IsPlayerUnit() && SelectionController.selectedUnit.HasInAttackRange(currentUnit))
                {
                    SelectionController.ShowTarget(currentUnit);
                    MovementTile.path = new List<Tile>();
                    MovementTile.path.Add(SelectionController.selectedUnit.currentTile);
                    MovementTile.DrawPath();
                    if (Input.GetMouseButtonDown(1))
                    {
                        SelectionController.selectedUnit.phase = UnitTurn.Attacking;
                        attackTile.GetComponent<AttackTile>().OnMouseOver();
                        return;
                    }
                }
            }
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                //left click - selection
                HexMap.ClearAllTiles();
                BattleController.instance.ResetColors();
                MovementTile.path = null;
                SelectionController.selectedTile = this;
                SelectionController.selectedUnit = currentUnit ? currentUnit : SelectionController.selectedUnit;
                if (currentUnit)
                {
                    //TO ADD: display stats
                    Unit.SaveAllStates();
                    if (currentUnit.IsPlayerUnit())
                    {
                        StatDisplay.DisplayPlayerUnit(currentUnit);
                        if (currentUnit.phase == UnitTurn.Open)
                        {
                            HexMap.ShowMovementTiles(currentUnit);
                            MovementTile.path = new List<Tile>() { this };
                        }

                    }
                    else
                    {
                        // show enemy mvt range and stats
                        StatDisplay.DisplayEnemyUnit(currentUnit);
                        HexMap.ShowMovementTiles(currentUnit);
                    }
                }
            }
        }
        
    }

    public void ShowMovementTile() {
        if (movementTile) {
            movementTile.transform.localPosition = GameResources.visibilityOffset;
            HexMap.showingMovementTiles.Push(this);
            HideAttackTile();
        }
    }

    public void HideMovementTile() {
        if (movementTile) {
            movementTile.transform.localPosition = -GameResources.visibilityOffset;
        }
    }
    public void ShowAttackTile() {
        if (attackTile) {
            attackTile.transform.localPosition = GameResources.visibilityOffset;
            HexMap.showingAttackTiles.Push(this);
            HideMovementTile();
        }
    }
    public void ShowAttackSprite()
    {
        if (attackTile)
        {
            attackTile.transform.localPosition = GameResources.visibilityOffset;
            HexMap.showingAttackTiles.Push(this);
            attackTile.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            HideMovementTile();
        }
    }
    public void HideAttackTile() {
        if (attackTile) {
            attackTile.gameObject.GetComponent<PolygonCollider2D>().enabled = true;
            attackTile.transform.localPosition = -GameResources.visibilityOffset;
        }
    }
}

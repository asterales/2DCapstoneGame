﻿using UnityEngine;
using MovementEffects;

public class AttackTile : MonoBehaviour {
    public Tile tile;

    public void OnMouseOver() {
        SelectionController sc = SelectionController.instance;
        TutorialController tutorial = BattleManager.instance.tutorial;
        if (sc.selectedUnit && sc.selectedUnit.phase == UnitTurn.Attacking) {
            if (sc.selectedUnit.IsPlayerUnit() && HasEnemyUnit() && sc.target != null) {
                sc.mode = SelectionMode.Attacking;
                if (tile.currentUnit != sc.target&&tile.currentUnit.phase==UnitTurn.Open){
                    sc.target.spriteRenderer.color = Color.white;
                    sc.ShowTarget(tile.currentUnit);
                }
                if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)){
                    sc.target = null;
                    Timing.RunCoroutine(sc.selectedUnit.PerformAttack(tile.currentUnit));
                    sc.HideTarget();
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (tile.currentUnit)
                {
                    //display tile stats
                }
                else
                {
                    // do regular tile selection
                    tile.OnMouseOver();
                }
            }
        } else if (tutorial && tutorial.enabled && tutorial.IsAttackTarget(this) && Input.GetMouseButtonDown(1)){
            sc.ShowTarget(tile.currentUnit);
        } else {
            tile.OnMouseOver();
        }
    }

    public void OnMouseEnter() { }


    private bool HasEnemyUnit() {
        return tile.currentUnit && !tile.currentUnit.IsPlayerUnit();
    }
    
}

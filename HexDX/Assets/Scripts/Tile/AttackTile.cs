using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AttackTile : MonoBehaviour {
    // to be implemented
    public Tile tile;

    public void OnMouseOver() {
        if (SelectionController.selectedUnit.phase == UnitTurn.Attacking) {
            if (Input.GetMouseButtonDown(0)) {
                if (tile.currentUnit) {
                    //display tile stats
                } else {
                    // do regular tile selection
                    tile.OnMouseOver();
                }
            } else if (SelectionController.selectedUnit.IsPlayerUnit() && HasEnemyUnit() && SelectionController.target != null) {
                SelectionController.mode = SelectionMode.Attacking;
                if (tile.currentUnit != SelectionController.target) {
                    SelectionController.target = tile.currentUnit;
                }
                if (HexMap.GetAttackTiles(tile.currentUnit).Contains(SelectionController.selectedUnit.currentTile))
                {
                    SelectionController.selectedUnit.GetComponent<SpriteRenderer>().color = Color.red;
                }
                else
                {
                    SelectionController.selectedUnit.GetComponent<SpriteRenderer>().color = Color.white;
                }
                if (Input.GetMouseButtonDown(1)){
                    SelectionController.target = null;
                    StartCoroutine(SelectionController.selectedUnit.PerformAttack(tile.currentUnit));
                    SelectionController.HideTarget();
                }
            }
        }
        else
        {
            tile.OnMouseOver();
        }
    }

    private bool HasEnemyUnit() {
        return tile.currentUnit && !tile.currentUnit.IsPlayerUnit();
    }
    
}

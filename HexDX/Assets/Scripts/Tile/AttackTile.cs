using UnityEngine;
using MovementEffects;

public class AttackTile : MonoBehaviour {
    public Tile tile;

    public void OnMouseOver() {
        if (SelectionController.selectedUnit && SelectionController.selectedUnit.phase == UnitTurn.Attacking) {
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
                if (Input.GetMouseButtonDown(1)){
                    SelectionController.target = null;
                    Timing.RunCoroutine(SelectionController.selectedUnit.PerformAttack(tile.currentUnit));
                    SelectionController.HideTarget();
                }
            }
        } else if (TutorialController.IsAttackTarget(this) && Input.GetMouseButtonDown(1)){
            SelectionController.target = tile.currentUnit;
        } else {
            tile.OnMouseOver();
        }
    }

    private bool HasEnemyUnit() {
        return tile.currentUnit && !tile.currentUnit.IsPlayerUnit();
    }
    
}

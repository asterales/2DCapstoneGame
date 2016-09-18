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
            } else if (Input.GetMouseButtonDown(1)
                            && SelectionController.selectedUnit.IsPlayerUnit()
                            && HasEnemyUnit()) {
                if (tile.currentUnit != SelectionController.target) {
                    SelectionController.target = tile.currentUnit;
                    SelectionController.mode = SelectionMode.Attacking;
                    if (HexMap.GetAttackTiles(tile).Contains(SelectionController.selectedUnit.currentTile)) {
                        SelectionController.selectedUnit.GetComponent<SpriteRenderer>().color = Color.red;
                    } else {
                        SelectionController.selectedUnit.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                } else {
                    StartCoroutine(SelectionController.selectedUnit.PerformAttack(tile.currentUnit));
                }
            }
        }
    }

    private bool HasEnemyUnit() {
        return tile.currentUnit && !tile.currentUnit.IsPlayerUnit();
    }
    
}

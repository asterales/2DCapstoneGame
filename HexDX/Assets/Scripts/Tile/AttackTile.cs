using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AttackTile : MonoBehaviour {
    // to be implemented
    public Tile tile;

    public void OnMouseOver() {

        if (SelectionController.IsMode(SelectionMode.Attacking)) {
            if (Input.GetMouseButtonDown(1)
                          && SelectionController.selectedUnit == PlayerBattleController.activeUnit
                          && HasEnemyUnit()) {
                if (tile.currentUnit != SelectionController.target) {
                    SelectionController.SetSelectedTarget(tile.currentUnit);
                    if (tile.currentUnit.HasInAttackRange(PlayerBattleController.activeUnit)) {
                        PlayerBattleController.activeUnit.GetComponent<SpriteRenderer>().color = Color.red;
                    } else {
                        PlayerBattleController.activeUnit.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                } else {
                    SelectionController.DisableTileSelection();
                    StartCoroutine(PlayerBattleController.activeUnit.PerformAttack(tile.currentUnit));
                }
            }
        } else if (SelectionController.IsMode(SelectionMode.Open)){
            tile.OnMouseOver();
            if (Input.GetMouseButtonDown(0))
            {
                if (tile.currentUnit)
                {
                    SelectionController.SetSelectedTile(tile);
                    //show attack tiles
                }
            }

        }

       
    }

    private bool HasEnemyUnit() {
        return tile.currentUnit && !tile.currentUnit.isPlayerUnit;
    }
    
}

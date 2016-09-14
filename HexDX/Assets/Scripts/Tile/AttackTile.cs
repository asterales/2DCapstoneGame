using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AttackTile : MonoBehaviour {
    // to be implemented
    public Tile tile;

    public void OnMouseOver() {
        if (SelectionController.selectedUnit.phase == UnitTurn.Attacking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (tile.currentUnit)
                {
                    //display unit stats
                }
                else
                {
                    // do regular tile selection
                    tile.OnMouseOver();
                }
            }
            else if (Input.GetMouseButtonDown(1)
                  && SelectionController.selectedUnit.isPlayerUnit
                  && tile.currentUnit != null
                  && !tile.currentUnit.isPlayerUnit)
            {
                if (tile.currentUnit != SelectionController.target)
                {
                    SelectionController.target = tile.currentUnit;
                    SelectionController.selectionMode = SelectionMode.Attacking;
                    if (HexMap.GetAttackTiles(tile).Contains(SelectionController.selectedUnit.currentTile))
                        SelectionController.selectedUnit.GetComponent<SpriteRenderer>().color = Color.red;
                    else
                        SelectionController.selectedUnit.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    StartCoroutine(SelectionController.selectedUnit.PerformAttack());
                }
                //SelectionController.selectedUnit.MakeAttacking();
                //tile.currentUnit.unitStats.health -= SelectionController.selectedUnit.unitStats.attack;
                //if (HexMap.GetAttackTiles(tile).Contains(SelectionController.selectedUnit.currentTile))
                //{
                //    SelectionController.selectedUnit.unitStats.health -= max((tile.currentUnit.unitStats.attack*2)/3, 1);
                //}
                //Image health = tile.currentUnit.transform.Find("HealthBar").GetComponent<Image>();
                //health.fillAmount = (float)tile.currentUnit.unitStats.health/ (float)tile.currentUnit.unitStats.maxHealth;
                //health = SelectionController.selectedUnit.transform.Find("HealthBar").GetComponent<Image>();
                //health.fillAmount = Mathf.Max(0,(float)SelectionController.selectedUnit.unitStats.health / (float)SelectionController.selectedUnit.unitStats.maxHealth);
                //if (SelectionController.selectedUnit.unitStats.health <= 0)
                //    Destroy(SelectionController.selectedUnit.gameObject);
                //if (tile.currentUnit.unitStats.health <= 0)
                //    Destroy(tile.currentUnit.gameObject);
            }
        }
    }

    
}

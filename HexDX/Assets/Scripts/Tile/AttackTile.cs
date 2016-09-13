using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AttackTile : MonoBehaviour {
    // to be implemented
    public Tile tile;
    public void OnMouseDown()
    {
        if (SelectionController.selectedUnit.isPlayerUnit && tile.currentUnit!=null && !tile.currentUnit.isPlayerUnit)
        {
            SelectionController.selectedUnit.MakeAttacking();
            tile.currentUnit.unitStats.health -= SelectionController.selectedUnit.unitStats.attack;
            if (HexMap.GetAttackTiles(tile).Contains(SelectionController.selectedUnit.currentTile))
            {
                SelectionController.selectedUnit.unitStats.health -= max((tile.currentUnit.unitStats.attack*2)/3, 1);
            }
            Image health = tile.currentUnit.transform.Find("HealthBar").GetComponent<Image>();
            health.fillAmount = (float)tile.currentUnit.unitStats.health/ (float)tile.currentUnit.unitStats.maxHealth;
            health = SelectionController.selectedUnit.transform.Find("HealthBar").GetComponent<Image>();
            health.fillAmount = Mathf.Max(0,(float)SelectionController.selectedUnit.unitStats.health / (float)SelectionController.selectedUnit.unitStats.maxHealth);
            if (SelectionController.selectedUnit.unitStats.health <= 0)
                Destroy(SelectionController.selectedUnit.gameObject);
            if (tile.currentUnit.unitStats.health <= 0)
                Destroy(tile.currentUnit.gameObject);
        }
    }

    private int max(int a, int b) { return a > b ? a : b; }
}

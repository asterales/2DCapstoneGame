using UnityEngine;
using System.Collections.Generic;

public class PlayerBattleController : MonoBehaviour {
    public List<Unit> units;
    
    // path sprites
    public Sprite circleSprite;
    public Sprite[] lineSprites;
    public Sprite[] arrowSprites;

    void Update(){
        if (SelectionController.selectedUnit != null 
                && SelectionController.selectedUnit.phase == UnitTurn.Facing 
                && Input.GetMouseButtonDown(0)) {
            SelectionController.selectedUnit.MakeDone();
        }
    }

    public void StartTurn() {
        SelectionController.selectionMode = SelectionMode.Open;
    }

    public void EndTurn() {
        for (int i = 0; i < units.Count; i++) {
            units[i].MakeOpen();
        }
    }

    public void AddUnit(Unit unit) {
        units.Add(unit);
    }
}

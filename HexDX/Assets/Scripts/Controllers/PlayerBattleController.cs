using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerBattleController : MonoBehaviour {
    public List<Unit> units;
    
    // path sprites
    public Sprite circleSprite;
    public Sprite[] lineSprites;
    public Sprite[] arrowSprites;

    void Start() {
        InitUnitList();
    }

    private void InitUnitList() {
        Unit[] allUnits = FindObjectsOfType(typeof(Unit)) as Unit[];
        units = allUnits.Where(unit => unit.isPlayerUnit).ToList();
    }


    void Update(){
        if (SelectionController.selectedUnit != null 
                && SelectionController.selectedUnit.phase == UnitTurn.Facing 
                && Input.GetMouseButtonDown(1)) {
            SelectionController.selectedUnit.MakeChoosingAction();
        }
    }

    public void StartTurn() {
        SelectionController.selectionMode = SelectionMode.Open;
    }

    public void EndTurn() {
        for (int i = 0; i < units.Count; i++) {
            if (units[i]){
                units[i].MakeOpen();
            } 
        }
    }

    public void AddUnit(Unit unit) {
        units.Add(unit);
    }
}

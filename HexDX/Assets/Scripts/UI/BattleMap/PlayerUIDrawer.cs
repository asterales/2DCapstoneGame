using UnityEngine;
using UnityEngine.UI;

public class PlayerUIDrawer : UnitUIDrawer {

	void Update () {
	    if (SelectionController.selectedUnit != null && SelectionController.selectedUnit.IsPlayerUnit()) {
            unit = SelectionController.selectedUnit;
        }
        if (SelectionController.target && SelectionController.target.IsPlayerUnit()) {
            unit = SelectionController.target;
        }
        DrawUI();
    }
}

using UnityEngine;
using System.Collections;
using System;

public class ScriptedFace : ScriptEvent {
    public Unit unit;
    public int direction;

	void Start () {
	    ////// DEBUG CODE //////
        if (unit == null)
        {
            Debug.Log("ERROR :: Unit needs to be defined -> ScriptedFace.cs");
        }
        ////////////////////////
	}

    void Update() {
        if (isActive && isPlayerEvent) {
            Vector2 directionVec = Input.mousePosition - Camera.main.WorldToScreenPoint(SelectionController.selectedUnit.transform.position);
            SelectionController.selectedUnit.SetFacing(directionVec);
            if (Input.GetMouseButtonDown(1) && SelectionController.selectedUnit.facing == direction) {
                SelectionController.selectedUnit = null;
                FinishEvent();
            }
        }
    }

    public override void DoPlayerEvent() {
        SelectionController.mode = SelectionMode.ScriptedPlayerFace;
        SelectionController.selectedUnit = unit;
    }

    public override void DoEvent() {
        SelectionController.mode = SelectionMode.ScriptedAI;
        unit.facing = direction;
        FinishEvent();
    }
}

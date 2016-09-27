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
            Vector2 directionVec = Input.mousePosition - CameraController.camera.WorldToScreenPoint(SelectionController.selectedUnit.transform.position);
            SelectionController.selectedUnit.SetFacing(directionVec);
            if (Input.GetMouseButtonDown(1) && SelectionController.selectedUnit.facing == direction) {
                FinishEvent();
            }
        }
    }

    public override void DoPlayerEvent() {
        SelectionController.mode = SelectionMode.ScriptedPlayerFace;
    }

    public override void DoEvent() {
        SelectionController.mode = SelectionMode.ScriptedAI;
        Debug.Log("AI is Facing");
    }
}

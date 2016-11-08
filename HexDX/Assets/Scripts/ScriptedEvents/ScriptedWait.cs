﻿using UnityEngine;

public class ScriptedWait : ScriptEvent {
    public Unit unit;

	protected override void Start () {
        base.Start();
	    ////// DEBUG CODE //////
        if (unit == null) {
            Debug.Log("ERROR :: Unit needs to be defined");
        }
        ////////////////////////
	}

    public override void DoPlayerEvent() {
        SelectionController.mode = SelectionMode.ScriptedPlayerWait;
        SelectionController.selectedUnit = null;
        unit.MakeDone();
        FinishEvent();
    }

    public override void DoEvent() {
        SelectionController.mode = SelectionMode.ScriptedAI;
        unit.MakeDone();
        FinishEvent();
    }
}

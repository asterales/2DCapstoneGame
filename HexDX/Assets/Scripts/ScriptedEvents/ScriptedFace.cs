﻿using UnityEngine;

public class ScriptedFace : ScriptEvent {
    public Unit unit;
    public int direction;

	protected override void Start () {
        base.Start();
	    ////// DEBUG CODE //////
        if (unit == null)
        {
            Debug.Log("ERROR :: Unit needs to be defined -> ScriptedFace.cs");
        }
        ////////////////////////
	}

    void Update() {
        if (isActive && isPlayerEvent) {
            list.sc.RegisterFacing();
            if (Input.GetMouseButtonDown(1) && list.sc.selectedUnit.facing == direction) {
                list.sc.selectedUnit = null;
                FinishEvent();
            }
        }
    }

    protected override void EarlyCleanUp() { 
        list.sc.selectedUnit = null;
    }

    public override void DoPlayerEvent() {
        list.sc.mode = SelectionMode.ScriptedPlayerFace;
        list.sc.selectedUnit = unit;
    }

    public override void DoEvent() {
        list.sc.mode = SelectionMode.ScriptedAI;
        unit.facing = direction;
        FinishEvent();
    }
}

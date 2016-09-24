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

    public override void StartEvent()
    {
        if (!playerEvent)
        {
            DoEvent();
            return;
        }
        SelectionController.mode = SelectionMode.ScriptedPlayerFace;
        // to be implemented
    }

    public override void DoEvent()
    {
        SelectionController.mode = SelectionMode.ScriptedAI;
        Debug.Log("AI is Facing");
        // to be implemented
    }

    public override void FinishEvent()
    {
        // to be implemented
        Complete();
    }
}

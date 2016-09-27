using UnityEngine;
using System.Collections;
using System;

public class ScriptedWait : ScriptEvent {
    public Unit unit;

	void Start () {
	    ////// DEBUG CODE //////
        if (unit == null)
        {
            Debug.Log("ERROR :: Unit needs to be defined");
        }
        ////////////////////////
	}

    public override void DoPlayerEvent() {
        SelectionController.mode = SelectionMode.ScriptedPlayerWait;
        // to be implemented
    }

    public override void DoEvent() {
        SelectionController.mode = SelectionMode.ScriptedAI;
        Debug.Log("AI is Waiting");
        // to be implemented
    }
}

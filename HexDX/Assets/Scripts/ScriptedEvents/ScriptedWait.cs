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

    public override void StartEvent()
    {
        if (!playerEvent)
        {
            DoEvent();
        }
        // to be implemented
    }

    public override void DoEvent()
    {
        // to be implemented
    }

    public override void FinishEvent()
    {
        // to be implemented
    }
}

using UnityEngine;
using System.Collections;
using System;

public class ScriptedCounter : ScriptEvent {
    public Unit attacker;
    public Unit victim;
    public int damageDone;

	void Start () {
	    ////// DEBUG CODE //////
        if (attacker == null)
        {
            Debug.Log("ERROR :: attacker needs to be defined -> ScriptedCounter.cs");
        }
        if (victim == null)
        {
            Debug.Log("ERROR :: victim needs to be defined -> ScriptedCounter.cs");
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

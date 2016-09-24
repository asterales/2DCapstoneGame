using UnityEngine;
using System.Collections;

public class ScriptedAttack : ScriptEvent {
    public Unit attacker;
    public Unit victim;
    public int damageDelt;

	void Start () {
	    ////// DEBUG CODE //////
        if (attacker == null)
        {
            Debug.Log("ERROR :: Attacker needs to be defined -> ScriptedAttack.cs");
        }
        if (victim == null)
        {
            Debug.Log("ERROR :: Victim needs to be defined -> ScriptedAttack.cs");
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

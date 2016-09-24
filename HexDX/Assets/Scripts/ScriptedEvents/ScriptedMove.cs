using UnityEngine;
using System.Collections;
using System;

public class ScriptedMove : ScriptEvent {
    public Unit unit;
    public MovementTile tile;

	void Start () {
	    ////// DEBUG CODE //////
        if (unit == null)
        {
            Debug.Log("ERROR :: Unit needs to be defined -> ScriptedMove.cs");
        }
        if (tile == null)
        {
            Debug.Log("ERROR :: Tile needs to be defined -> ScriptedMove.cs");
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
}

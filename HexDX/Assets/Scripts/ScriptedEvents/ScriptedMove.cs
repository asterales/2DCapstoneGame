using UnityEngine;
using System.Collections;
using System;

public class ScriptedMove : ScriptEvent {
    public Unit unit;
    public Tile tile;
    private MovementTile movement;

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
            return;
        }
        SelectionController.mode = SelectionMode.ScriptedPlayerMove;
        // to be implemented
    }

    public override void DoEvent()
    {
        SelectionController.mode = SelectionMode.ScriptedAI;
        Debug.Log("AI is Moving");
        unit.SetPath(unit.GetShortestPath(tile));
        unit.MakeMoving(this);
        // to be implemented
    }

    public override void FinishEvent()
    {
        // to be implemented
        Complete();
    }
}

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

    public override void DoPlayerEvent() {
        SelectionController.mode = SelectionMode.ScriptedPlayerMove;
        TutorialController.targetTile = tile;
        SelectionController.selectedTile = unit.currentTile;
        SelectionController.selectedUnit = unit;
        Debug.Log("Phase: " + unit.phase);
        // let movement tile selection take care of itself
    }

    public override void DoEvent() {
        SelectionController.mode = SelectionMode.ScriptedAI;
        Debug.Log("AI is Moving");
        unit.SetPath(unit.GetShortestPath(tile));
        unit.MakeMoving(this);
    }

    public override void FinishEvent(){
        TutorialController.targetTile = null;
        SelectionController.selectedTile = null;
        SelectionController.selectedUnit = null;
        base.FinishEvent();
    }
}

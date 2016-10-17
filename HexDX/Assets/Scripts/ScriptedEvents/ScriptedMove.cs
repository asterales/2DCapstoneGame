using UnityEngine;
using System.Collections;
using System;

public class ScriptedMove : ScriptEvent {
    public Unit unit;
    public Tile tile;
    public int tileRow;
    public int tileCol;
    public HexMap reference;

	void Start () {
	    ////// DEBUG CODE //////
        if (unit == null)
        {
            Debug.Log("ERROR :: Unit needs to be defined -> ScriptedMove.cs");
        }
        //if (tile == null)
        //{
        //    Debug.Log("ERROR :: Tile needs to be defined -> ScriptedMove.cs");
        //}
        if (reference == null)
        {
            Debug.Log("ERROR :: Reference to HexMap needs to be defined -> ScriptedMove.cs");
        }
        ////////////////////////
        tile = null;
        tile = HexMap.mapArray[tileRow][tileCol];
        if (tile == null)
        {
            Debug.Log("ERROR :: HexMap should initialize before ScriptedMoves -> ScriptedMove.cs");
        }
	}

    public override void DoPlayerEvent() {
        SelectionController.mode = SelectionMode.ScriptedPlayerMove;
        TutorialController.targetTile = tile;
        SelectionController.selectedTile = unit.currentTile;
        SelectionController.selectedUnit = unit;
        // let movement tile selection take care of itself
    }

    public override void DoEvent() {
        SelectionController.mode = SelectionMode.ScriptedAI;
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

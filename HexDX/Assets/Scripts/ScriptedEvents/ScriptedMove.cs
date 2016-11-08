using UnityEngine;

public class ScriptedMove : ScriptEvent {
    public Unit unit;
    public Tile tile;
    public int tileRow;
    public int tileCol;

	protected override void Start () {
        base.Start();
	    ////// DEBUG CODE //////
        if (unit == null)
        {
            Debug.Log("ERROR :: Unit needs to be defined -> ScriptedMove.cs");
        }
        ////////////////////////
	}

    public override void StartEvent() {
        tile = HexMap.mapArray[tileRow][tileCol];
        base.StartEvent();
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

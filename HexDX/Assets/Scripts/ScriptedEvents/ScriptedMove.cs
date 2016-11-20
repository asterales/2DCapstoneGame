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
        list.sc.mode = SelectionMode.ScriptedPlayerMove;
        list.tutorial.targetTile = tile;
        list.sc.selectedTile = unit.currentTile;
        list.sc.selectedUnit = unit;
        // let movement tile selection take care of itself
    }

    public override void DoEvent() {
        list.sc.mode = SelectionMode.ScriptedAI;
        unit.SetPath(unit.GetShortestPath(tile));
        unit.MakeMoving(this);
    }

    public override void FinishEvent(){
        list.tutorial.targetTile = null;
        list.sc.selectedTile = null;
        list.sc.selectedUnit = null;
        base.FinishEvent();
    }
}

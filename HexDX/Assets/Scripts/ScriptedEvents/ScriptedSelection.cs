using UnityEngine;

public class ScriptedSelection : ScriptEvent {
	/* Coordinates of tile to select */
	public int row;
	public int col;
	
	private Tile targetTile {
		get { return list.tutorial.targetTile; }
		set { list.tutorial.targetTile = value; }
	}

	void Update() {
		if(list.sc.selectedTile == targetTile) {
			ShowSelection();
			targetTile = null;
			list.sc.selectedTile = null;
			FinishEvent();
		}
	}

	protected override void EarlyCleanUp() {
		targetTile = null;
		list.sc.selectedTile = null;
	}

	public override void DoEvent() {
		FinishEvent();
	}

    public override void DoPlayerEvent() {
    	list.sc.mode = SelectionMode.ScriptedPlayerSelection;
    	targetTile = HexMap.mapArray[row][col];
    }

    private void ShowSelection() {
    	list.sc.ShowSelection(targetTile);
		if (targetTile.currentUnit){
			list.sc.ShowSelection(targetTile.currentUnit);
		}
    }
}
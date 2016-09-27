using UnityEngine;
using System.Collections;

public class ScriptedSelection : ScriptEvent {
	/* Coordinates of tile to select */
	public int row;
	public int col;
	
	private Tile targetTile {
		get { return TutorialController.targetTile; }
		set { TutorialController.targetTile = value; }
	}

	void Update() {
		if(isActive && SelectionController.selectedTile == targetTile) {
			ShowSelection();
			targetTile = null;
			SelectionController.selectedTile = null;
			FinishEvent();
		}
	}

	public override void DoEvent() {
		FinishEvent();
	}

    public override void DoPlayerEvent() {
    	SelectionController.mode = SelectionMode.ScriptedPlayerSelection;
    	targetTile = HexMap.mapArray[row][col];
    }

    private void ShowSelection() {
    	SelectionController.ShowSelection(targetTile);
		if (targetTile.currentUnit){
			SelectionController.ShowSelection(targetTile.currentUnit);
		}
    }
}
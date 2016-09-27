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
			SetSelection();
			TutorialController.HideSelectionPrompt();
			FinishEvent();
		}
	}

	public override void DoEvent() {
		FinishEvent();
	}

    public override void DoPlayerEvent() {
    	SelectionController.mode = SelectionMode.ScriptedPlayerSelection;
    	targetTile = HexMap.mapArray[row][col];
    	TutorialController.ShowSelectionPrompt(targetTile);
    }

    private void SetSelection() {
    	SelectionController.ShowSelection(targetTile);
		if (targetTile.currentUnit){
			SelectionController.selectedUnit = targetTile.currentUnit;
			SelectionController.ShowSelection(targetTile.currentUnit);
		}
    }
}
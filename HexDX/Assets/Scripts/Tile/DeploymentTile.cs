using UnityEngine;
using System.Collections.Generic;

public class DeploymentTile : MonoBehaviour {
	public Tile tile;

	public void OnMouseOver() {
		if (SelectionController.mode == SelectionMode.DeploymentOpen) {
			if (Input.GetMouseButtonDown(0)) {
				SelectUnit();
			} else if (Input.GetMouseButtonDown(1)) {
				SelectMoveDestination();
			}
		}
	}

	private void SelectUnit() {
		if (tile.currentUnit) {
			if (SelectionController.selectedUnit) {
				SelectionController.selectedUnit.MakeOpen();
			}
			SelectionController.selectedUnit = tile.currentUnit;
		}
	}

	private void SelectMoveDestination() {
		if (SelectionController.selectedUnit 
				&& SelectionController.selectedUnit.phase == UnitTurn.Moving) {
			if (tile.currentUnit != SelectionController.selectedUnit) {
				DeploymentController.SetSelectedUnitDestination(tile);
			} else {
				SelectionController.selectedUnit.MakeOpen();
				SelectionController.selectedUnit = null;
			}
		}
	}
}

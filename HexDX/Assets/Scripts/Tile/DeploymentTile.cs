using UnityEngine;
using System.Collections;

public class DeploymentTile : MonoBehaviour {
	public Tile tile;

	public void OnMouseOver() {
		if (SelectionController.mode == SelectionMode.Deployment) {
			if (Input.GetMouseButtonDown(0)) {
				SelectUnit();
			} else if (Input.GetMouseButtonDown(1)) {
				MoveSelectedUnit();
			}
		}
	}

	private void SelectUnit() {
		if (tile.currentUnit) {
			SelectionController.selectedUnit = tile.currentUnit;
			SelectionController.ShowSelection(tile);
		}
	}

	private void MoveSelectedUnit() {
		if (SelectionController.selectedUnit && tile.currentUnit != SelectionController.selectedUnit) {
			
		}
	}
}

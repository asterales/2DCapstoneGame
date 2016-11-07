using UnityEngine;
using System.Collections.Generic;

public class DeploymentTile : MonoBehaviour {
	private static Unit draggedUnit;
	private static Tile hoveredTile;

	public Tile tile;

	void Awake() {
		ClearDragVariables();
	}

	void OnDestroy() {
		ClearDragVariables();
	}

	void OnMouseDown() {
		if (tile.currentUnit && SelectionController.mode == SelectionMode.DeploymentOpen) {
			draggedUnit = tile.currentUnit;
		}
	}

	void OnMouseOver() {
		if (draggedUnit) {
			hoveredTile = tile;
		}
	}

	void OnMouseExit() {
		hoveredTile = null;
	}

	void OnMouseDrag() {
		if (draggedUnit) {
			Vector3 cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			float z;
			if (hoveredTile) {
				z = hoveredTile.transform.position.z;
			} else {
				z = draggedUnit.transform.position.z;
			}
			draggedUnit.transform.position = new Vector3(cameraPos.x, cameraPos.y, z);
		}
	}

	void OnMouseUp() {
		if (draggedUnit) {
			if (hoveredTile) {
				if (hoveredTile.currentUnit) {
					DeploymentController.SetUnitForDisplacement(hoveredTile.currentUnit, draggedUnit.currentTile);
				}
				draggedUnit.SetTile(hoveredTile);
				DeploymentController.SetForDeploymentFacing(draggedUnit);
			} else {
				draggedUnit.SetTile(draggedUnit.currentTile);
			}
			draggedUnit = null;
		}
	}

	void Update() {
		if (draggedUnit) {
			if (hoveredTile) {
				SelectionController.ShowSelection(hoveredTile);
			} else {
				SelectionController.ShowSelection(draggedUnit.currentTile);
			} 
		} else {
			SelectionController.HideSelection();
		}
	}

	private void ClearDragVariables() {
		draggedUnit = null;
		hoveredTile = null;
	}

}

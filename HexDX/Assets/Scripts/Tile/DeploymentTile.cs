using UnityEngine;

public class DeploymentTile : MonoBehaviour {
	private static Unit draggedUnit;
	private static Tile hoveredTile;

	public Tile tile;
	private SelectionController sc;

	void Awake() {
		ClearDragVariables();
	}

	void Start() {
		sc = SelectionController.instance;
	}

	void OnDestroy() {
		ClearDragVariables();
	}

	void OnMouseDown() {
		if (tile.currentUnit && sc.mode == SelectionMode.DeploymentOpen) {
			GameManager.instance.PlayCursorSfx();
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
			DeploymentController dc = BattleManager.instance.deploymentController;
			if (hoveredTile) {
				GameManager.instance.PlayCursorSfx();
				dc.SetSelectedUnitDestination(draggedUnit, hoveredTile);
			} else {
				dc.SetSelectedUnitDestination(draggedUnit, draggedUnit.currentTile);
			}
			ClearDragVariables();
		}
	}

	void Update() {
		if (sc.mode == SelectionMode.DeploymentOpen) {
			if (hoveredTile) {
				sc.ShowSelection(hoveredTile);
			} else if (draggedUnit) {
				sc.ShowSelection(draggedUnit.currentTile);
			} else {
				sc.HideSelection();
			}
		}
	}

	private void ClearDragVariables() {
		draggedUnit = null;
		hoveredTile = null;
	}

}

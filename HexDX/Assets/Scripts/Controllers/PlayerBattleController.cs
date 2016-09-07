using UnityEngine;
using System.Collections;

public class PlayerBattleController : MonoBehaviour {
	public bool FinishedTurn { get; set; }	
	private SelectionController selectionController;
	private Tile unitTile;
	private Tile destinationTile;
	public int unitTranslationSpeed = 5;

	void Start(){
		FinishedTurn = false;
		selectionController = GameObject.Find("HexMap").GetComponent<HexMap>().GetComponent<SelectionController>();
	}

	void Update(){
		if(unitTile && destinationTile){
			MoveUnit();
		} else {
			CheckTileSelection();
		}
	}

	private bool InMovementRange(Tile unitTile, Tile dest){
		int speed = unitTile.currentUnit.unitStats.speed;
		int rowDiff = (int) Mathf.Abs(dest.position.row - unitTile.position.row);
		int colDiff = (int) Mathf.Abs(dest.position.col - unitTile.position.col);
		return rowDiff <= speed && colDiff <= speed;
	}

	private void MoveUnit() {
		GameObject unitObj = unitTile.currentUnit.gameObject;
		Vector3 distLeft = destinationTile.transform.position - unitObj.transform.position;
		if (distLeft.magnitude == 0){
			Unit unit = unitTile.currentUnit;
			destinationTile.currentUnit = unit;
			unit.currentTile = destinationTile;
			unitTile.currentUnit = null;
			unitObj.transform.parent = destinationTile.transform;
			unitObj.transform.localPosition = new Vector3(0, 0, 0);
			unitTile = null;
			destinationTile = null;
			FinishedTurn = true;
		} else {
			//walk rows, then columns (to be changed)
			if(distLeft.x != 0) {
				int direction = distLeft.x < 0 ? -1 : 1;
				float xMvt = Mathf.Min(Mathf.Abs(distLeft.x), unitTranslationSpeed * Time.deltaTime );
				unitObj.transform.position += new Vector3(direction * xMvt, 0, 0);
			} else if (distLeft.y != 0) {
				int direction = distLeft.y < 0 ? -1 : 1;
				float yMvt = Mathf.Min(Mathf.Abs(distLeft.y), unitTranslationSpeed * Time.deltaTime );
				unitObj.transform.position += new Vector3(0, direction * yMvt, 0);
			}
		}

	}

	private void CheckTileSelection() {
		Tile selectedTile = selectionController.selectedTile;
		if (selectedTile && selectedTile.currentUnit){
			unitTile = selectedTile;
		} else if (unitTile && !destinationTile){
			if (InMovementRange(unitTile, selectedTile) && selectedTile.pathable){
				destinationTile = selectedTile;
			}
		}
	}
}

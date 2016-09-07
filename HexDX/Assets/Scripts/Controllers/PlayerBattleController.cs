using UnityEngine;
using System.Collections;

public class PlayerBattleController : MonoBehaviour {
	public bool FinishedTurn { get; set; }	
	private SelectionController selectionController;
	private Tile unitTile;
	private Tile destinationTile;

	void Start(){
		FinishedTurn = false;
		selectionController = GameObject.Find("TestHexMap").GetComponent<HexMap>().GetComponent<SelectionController>();
	}

	void Update(){
	}

	private void MoveUnit() {
		bool reachedDestination = true;
		if (reachedDestination){
			unitTile = null;
        	destinationTile = null;
        	selectionController.ClearSelection();
        	FinishedTurn = true;
		}
	}

	private void CheckTileSelection() {
		Tile selectedTile = selectionController.selectedTile;
		if (selectedTile && selectedTile.currentUnit){
			unitTile = selectedTile;
		} else if (unitTile && !destinationTile){
			if (unitTile.currentUnit.IsReachableTile(selectedTile)){
				destinationTile = selectedTile;
			}
		}
	}
}

using UnityEngine;
using System.Collections.Generic;

public class PlayerBattleController : MonoBehaviour {
	public bool FinishedTurn { get; set; }	
	private SelectionController selectionController;
	private Tile unitTile;
	private Tile destinationTile;
    
    // path sprites
    public Sprite circleSprite;
    public Sprite[] lineSprites;
    public Sprite[] arrowSprites;

	void Start(){
		FinishedTurn = false;
		selectionController = GameObject.Find("TestHexMap").GetComponent<HexMap>().GetComponent<SelectionController>();
	}

	void Update(){
        //if (MovementTile.path.Count > 1)
        //{
        if (MovementTile.path!=null)
        {
            if (!Input.GetMouseButton(0))
            {
                SelectionController.selectedUnit.path = new Queue<Tile>(MovementTile.path);
                MovementTile.path = null;
                HexMap.ClearMovementTiles();
                selectionController.ClearSelection();
            }
        }
        //}
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
		if (SelectionController.selectedTile && SelectionController.selectedUnit)
        {
			unitTile = SelectionController.selectedTile;
		} 
	}
}

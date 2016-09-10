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
        if (!Input.GetMouseButton(0) && MovementTile.path != null) {
            SelectionController.selectedUnit.path = new Queue<Tile>(MovementTile.path);
            MovementTile.path = null;
            HexMap.ClearMovementTiles();
            selectionController.ClearSelection();
        }
    }
}

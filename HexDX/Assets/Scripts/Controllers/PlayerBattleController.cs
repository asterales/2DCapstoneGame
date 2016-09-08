using UnityEngine;
using System.Collections.Generic;

public class PlayerBattleController : MonoBehaviour {
	public bool FinishedTurn { get; set; }	
	private SelectionController selectionController;
	private Tile unitTile;
	private Tile destinationTile;
    public LineRenderer pathDrawer;

	void Start(){
		FinishedTurn = false;
		selectionController = GameObject.Find("TestHexMap").GetComponent<HexMap>().GetComponent<SelectionController>();
        pathDrawer = gameObject.AddComponent<LineRenderer>();
	}

	void Update(){
        //if (MovementTile.path.Count > 1)
        //{
        if (MovementTile.path!=null)
        {
            pathDrawer.SetVertexCount(MovementTile.path.Count);
            pathDrawer.SetPositions(MovementTile.path.ToArray());
            if (!Input.GetMouseButton(0))
            {
                SelectionController.selectedUnit.path = new Queue<Vector3>(MovementTile.path);
                MovementTile.path = null;
                pathDrawer.SetVertexCount(0);
                pathDrawer.SetPositions(new Vector3[0]);
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

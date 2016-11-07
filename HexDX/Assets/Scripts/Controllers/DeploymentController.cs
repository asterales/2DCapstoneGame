using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DeploymentController : PreBattleController {
	// For disabling specifc HUD elements
	private static readonly List<string> disabledHudElementNames = new List<string> { 
		//"EndTurnButton", "EndBanners", "TurnBanners", "TileUI", "EnemyUI"
		"EndTurnButton", "HUD"
	};

	private List<GameObject> disabledHudElements;
	private GameObject deploymentUI;
	private List<DeploymentTile> deploymentTiles;
	
	private static Tile selectedUnitDest;
	private static bool faceAfterMove;
	private static Unit displacedUnit;
	private static Tile displacedUnitDest;

	public float maxMovement;

	protected override void Awake() {
		base.Awake();
		deploymentTiles = new List<DeploymentTile>();
		deploymentUI = GameObject.Find("Deployment UI");
		disabledHudElements = disabledHudElementNames.Select(n => GameObject.Find(n)).ToList();
	}

	void OnDestroy() {
		ClearSelections();
	}

	public override void StartPreBattlePhase() {
		base.StartPreBattlePhase();
		if (deploymentTiles.Count > 0) {
			disabledHudElements.ForEach(d => d.SetActive(false));
			ClearSelections();
			SelectionController.mode = SelectionMode.DeploymentOpen;
		} else {
			EndPreBattlePhase();
		}
	}

	// Used by MapLoader
	public void LoadDeploymentTiles(List<Tile> tiles) {
		foreach(Tile tile in tiles) {
			GameObject deployTileObj = Instantiate(Resources.Load("Tiles/DeploymentTile")) as GameObject;
	        deployTileObj.transform.parent = tile.transform;
	        deployTileObj.transform.localPosition = GameResources.visibilityOffset;
	        DeploymentTile deployTile = deployTileObj.GetComponent<DeploymentTile>();
	        deployTile.tile = tile;
	        deploymentTiles.Add(deployTile);
		}
	}

	public static void SetSelectedUnitDestination(Unit selectedUnit, Tile destTile) {
		selectedUnitDest = destTile;
		faceAfterMove = selectedUnitDest != selectedUnit.currentTile;
		if (destTile.currentUnit && destTile.currentUnit != selectedUnit) {
			displacedUnit = destTile.currentUnit;
			displacedUnitDest = selectedUnit.currentTile;
		}
		SelectionController.selectedUnit = selectedUnit;
		SelectionController.mode = SelectionMode.DeploymentMove;
	}

	private static void ClearSelections() {
		selectedUnitDest = null;
		displacedUnit = null;
		displacedUnitDest = null;
	}

	protected override void PhaseUpdateAction() {
		if (SelectionController.selectedUnit) {
			if (selectedUnitDest) {
				SelectionController.ShowSelection(selectedUnitDest);
				MoveUnit(SelectionController.selectedUnit, selectedUnitDest);
			} else {
				SelectionController.ShowSelection(SelectionController.selectedUnit);
				FaceSelectedUnit();
			}
		}
		if (displacedUnit) {
			MoveUnit(displacedUnit, displacedUnitDest);
		}
	}

	private void MoveUnit(Unit unit, Tile destTile) {
		SelectionController.mode = SelectionMode.DeploymentMove;
    	Vector3 destination = destTile.transform.position;
    	Vector3 unitPos = unit.transform.position;
    	if (unitPos != destination) {
    		unit.transform.position = Vector3.MoveTowards(unitPos, destination, maxMovement);
    	} else {
    		unit.SetTile(destTile);
    		unit.MakeOpen();
    		if (destTile == selectedUnitDest) {
    			if (faceAfterMove) {
    				SelectionController.selectedUnit.MakeFacing();
    			} else {
    				SelectionController.selectedUnit = null;
    			}
    			selectedUnitDest = null;
    		} else if (unit == displacedUnit) {
    			displacedUnit = null;
    			displacedUnitDest = null;
    		}
    		if(selectedUnitDest == null && displacedUnitDest == null) {
    			SelectionController.mode = SelectionMode.DeploymentOpen;
    		}
    	}
    }

	private void FaceSelectedUnit() {
		SelectionController.mode = SelectionMode.DeploymentFace;
		SelectionController.RegisterFacing();
		if(Input.GetMouseButtonDown(1)) {
			HexMap.ClearAttackTiles();
			SelectionController.selectedUnit.MakeOpen();
			SelectionController.selectedUnit = null;
			SelectionController.HideSelection();
			SelectionController.mode = SelectionMode.DeploymentOpen;
		}
	}

	public override void EndPreBattlePhase() {
		// Make all units open and destroy deployment tiles
		foreach(DeploymentTile deployTile in deploymentTiles) {
			if(deployTile.tile.currentUnit) {
				deployTile.tile.currentUnit.MakeOpen();
			}
			Destroy(deployTile.gameObject);
		}
		HexMap.ClearAllTiles();

		// Reset HUD
		deploymentUI.SetActive(false);
		disabledHudElements.ForEach(h => h.SetActive(true));

		base.EndPreBattlePhase();
	}
}


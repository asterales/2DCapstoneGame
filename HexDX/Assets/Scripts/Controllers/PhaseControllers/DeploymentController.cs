using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DeploymentController : PhaseController {
	// For disabling specifc HUD elements
	private static readonly List<string> disabledHudElementNames = new List<string> { 
		//"EndTurnButton", "EndBanners", "TurnBanners", "TileUI", "EnemyUI"
		"EndTurnButton", "HUD"
	};

	private List<GameObject> disabledHudElements;
	private GameObject deploymentUI;
	private List<DeploymentTile> deploymentTiles;
	
	private Tile selectedUnitDest;
	private Unit displacedUnit;
	private Tile displacedUnitDest;

	public float maxMovement;

	protected override void Awake() {
		deploymentTiles = new List<DeploymentTile>();
		deploymentUI = GameObject.Find("Deployment UI");
		disabledHudElements = disabledHudElementNames.Select(n => GameObject.Find(n)).ToList();
		base.Awake();
	}

	public override void StartBattlePhase() {
		base.StartBattlePhase();
		if (deploymentTiles.Count > 0) {
			disabledHudElements.ForEach(d => d.SetActive(false));
			ClearSelections();
			sc.mode = SelectionMode.DeploymentOpen;
		} else {
			EndBattlePhase();
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

	public void SetSelectedUnitDestination(Unit selectedUnit, Tile destTile) {
		selectedUnitDest = destTile;
		if (destTile.currentUnit && destTile.currentUnit != selectedUnit) {
			displacedUnit = destTile.currentUnit;
			displacedUnitDest = selectedUnit.currentTile;
		}
		sc.selectedUnit = selectedUnit;
		sc.mode = SelectionMode.DeploymentMove;
	}

	private void ClearSelections() {
		selectedUnitDest = null;
		displacedUnit = null;
		displacedUnitDest = null;
	}

	void Update() {
		if (sc.selectedUnit) {
			sc.ShowSelection(selectedUnitDest);
			MoveUnit(sc.selectedUnit, selectedUnitDest);
		}
		if (displacedUnit) {
			MoveUnit(displacedUnit, displacedUnitDest);
		}
	}

	private void MoveUnit(Unit unit, Tile destTile) {
		sc.mode = SelectionMode.DeploymentMove;
    	Vector3 destination = destTile.transform.position;
    	Vector3 unitPos = unit.transform.position;
    	if (unitPos != destination) {
    		unit.transform.position = Vector3.MoveTowards(unitPos, destination, maxMovement);
    	} else {
    		unit.SetTile(destTile);
    		unit.MakeOpen();
    		if (destTile == selectedUnitDest) {
    			sc.selectedUnit.MakeOpen();
				sc.selectedUnit = null;
				sc.HideSelection();
    			selectedUnitDest = null;
    		} else if (unit == displacedUnit) {
    			displacedUnit = null;
    			displacedUnitDest = null;
    		}
    		if(selectedUnitDest == null && displacedUnitDest == null) {
    			sc.mode = SelectionMode.DeploymentOpen;
    		}
    	}
    }

	public override void EndBattlePhase() {
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

		base.EndBattlePhase();
	}
}


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
	private static Unit displacedUnit;
	private static Tile displacedUnitDest;

	private static readonly float maxMovement = 0.35f;

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

	private static void ClearSelections() {
		selectedUnitDest = null;
		displacedUnit = null;
		displacedUnitDest = null;
	}

	protected override void PhaseUpdateAction() {
		DisplaySelectedUnit();
		if (SelectionController.selectedUnit) {
			if (selectedUnitDest) {
				MoveUnit(SelectionController.selectedUnit, selectedUnitDest);
			} else if (SelectionController.selectedUnit.phase == UnitTurn.Facing) {
				FaceSelectedUnit();
			}
		}
		if (displacedUnit) {
			MoveUnit(displacedUnit, displacedUnitDest);
		}
	}

	private void DisplaySelectedUnit() {
		if(SelectionController.selectedUnit) {
			SelectionController.ShowSelection(SelectionController.selectedUnit);
		} else {
			SelectionController.HideSelection();
		}
	}

	private void MoveUnit(Unit unit, Tile destTile) {
    	Vector3 destination = destTile.transform.position;
    	Vector3 unitPos = unit.transform.position;
    	if (unitPos != destination) {
    		unit.transform.position = Vector3.MoveTowards(unitPos, destination, maxMovement);
    	} else {
    		unit.SetTile(destTile);
    		unit.MakeOpen();
    		if (destTile == selectedUnitDest) {
    			SelectionController.selectedUnit = null;
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
			SelectionController.mode = SelectionMode.DeploymentOpen;
		}
	}

	void OnGUI() {
        if (SelectionController.selectedUnit && SelectionController.selectedUnit.phase == UnitTurn.Open) {
            int itemHeight = 20;
            int itemWidth = 60;
            int offset = 60;
            Vector3 pos = Camera.main.WorldToScreenPoint(SelectionController.selectedUnit.transform.position);
            pos = new Vector3(pos.x, Screen.height - pos.y-offset);

            if (GUI.Button(new Rect(pos.x, pos.y, itemWidth, itemHeight), " Move", player.GetGUIStyle(true))) {
                SelectionController.selectedUnit.MakeMoving();
            }
            if (GUI.Button(new Rect(pos.x, pos.y+ itemHeight, itemWidth, itemHeight), " Face", player.GetGUIStyle(true))) {
                SelectionController.selectedUnit.MakeFacing();
            }
        }
    }

	public static void SetSelectedUnitDestination(Tile destTile) {
		selectedUnitDest = destTile;
		if (destTile.currentUnit) {
			displacedUnit = destTile.currentUnit;
			displacedUnitDest = SelectionController.selectedUnit.currentTile;
		}
		SelectionController.mode = SelectionMode.DeploymentMove;
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


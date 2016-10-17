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

	private ScriptList preBattleComments; //pull this out later, currently added for prebattle comments
	
	private static Tile selectedUnitDest;
	private static Unit displacedUnit;
	private static Tile displacedUnitDest;

	private static readonly float maxMovement = 0.35f;

	void Awake() {
		preBattleComments = FindObjectOfType(typeof(ScriptList)) as ScriptList;
		if (preBattleComments && 
				preBattleComments.scriptedEvents.Where(e => e.GetType() != typeof(ScriptedDialogue)).ToList().Count > 0) {
			Debug.Log("ERROR: cannot have other scripted events other than ScriptedDialogues in ScriptList - DeploymentController.cs");
			preBattleComments = null;
		} else {
			Character tutorialAdvisor = Character.characters[2]; // Colonel Schmidt
			preBattleComments.dialogueMgr.SetSpeaker(tutorialAdvisor, 7);
		}

		deploymentTiles = new List<DeploymentTile>();
		deploymentUI = GameObject.Find("Deployment UI");
		disabledHudElements = disabledHudElementNames.Select(n => GameObject.Find(n)).ToList();
	}

	protected override void Start() {
		base.Start();
		LoadActiveUnits();
		disabledHudElements.ForEach(d => d.SetActive(false));
		if (preBattleComments) {
			preBattleComments.StartEvents();
		} else {
			SelectionController.mode = SelectionMode.DeploymentOpen;
		}
	}

	private void LoadActiveUnits() {
		List<Unit> activeUnits = GameManager.instance.activeUnits;
		if (activeUnits != null) {
			for(int i = 0; i < activeUnits.Count; i++) {
				activeUnits[i].SetTile(deploymentTiles[i].tile);
			}
		}
	}

	void Update() {
		if (preBattleComments && preBattleComments.EventsCompleted) {
			SelectionController.mode = SelectionMode.DeploymentOpen;
			preBattleComments.dialogueMgr.HideGUI();
			preBattleComments = null;
		} else {
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

	// Used with MapLoader
	public void AddDeploymentTile(int row, int col) {
		Tile tile = HexMap.mapArray[row][col];
        GameObject deployTileObj = Instantiate(Resources.Load("Tiles/DeploymentTile")) as GameObject;
        deployTileObj.transform.parent = tile.transform;
        deployTileObj.transform.localPosition = GameResources.visibilityOffset;
        DeploymentTile deployTile = deployTileObj.GetComponent<DeploymentTile>();
        deployTile.tile = tile;
        deploymentTiles.Add(deployTile);
	}

	public override void EndPreBattlePhase() {
		deploymentTiles.ForEach(d => Destroy(d.gameObject));
		deploymentUI.SetActive(false);
		disabledHudElements.ForEach(h => h.SetActive(true));
		base.EndPreBattlePhase();
	}
}


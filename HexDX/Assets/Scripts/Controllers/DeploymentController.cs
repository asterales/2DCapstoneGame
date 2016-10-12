using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DeploymentController : PreBattleController {
	private GameObject hud;
	private GameObject endTurnButton;
	private GameObject deploymentUI;
	private List<DeploymentTile> deploymentTiles;
	public static Unit selectedUnit;

	void Awake() {
		deploymentTiles = new List<DeploymentTile>();
		deploymentUI = GameObject.Find("Deployment UI");
		hud = GameObject.Find("HUD");
		endTurnButton = GameObject.Find("EndTurnButton");
	}

	public void AddDeploymentTile(int row, int col) {
		Tile tile = HexMap.mapArray[row][col];
        GameObject deployTileObj = Instantiate(Resources.Load("Tiles/DeploymentTile")) as GameObject;
        deployTileObj.transform.parent = tile.transform;
        deployTileObj.transform.localPosition = GameResources.visibilityOffset;
        DeploymentTile deployTile = deployTileObj.GetComponent<DeploymentTile>();
        deployTile.tile = tile;
        deploymentTiles.Add(deployTile);
	}

	protected override void Start() {
		base.Start();
		SelectionController.mode = SelectionMode.Deployment;
		hud.SetActive(false);
		endTurnButton.SetActive(false);
	}

	public override void EndPreBattlePhase() {
		deploymentTiles.ForEach(d => Destroy(d.gameObject));
		hud.SetActive(true);
		endTurnButton.SetActive(true);
		deploymentUI.SetActive(false);
		base.EndPreBattlePhase();
	}
}


using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BattleControllerManager : MonoBehaviour {
	// singleton
	public static BattleControllerManager instance;

	// Default controllers set in editor
	public MapLoader mapLoader;
	public CameraController cameraController;
	public DeploymentController deploymentController;
	public BattleController battleController;
	public PlayerBattleController player;
	public AIBattleController ai;

	// Imported Controllers
	public TutorialController tutorialController;
	public ScriptedAIBattleController scriptedAI;
	
	// Prebattle phase management
	public List<PreBattleController> prebattlePhases;
	private int preBattlePhaseIndex;
	

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

	void Start() {
		InitBattleScene();
		StartBattlePhases();
	}

	private void InitBattleScene() {
		GetImportedControllers();
		mapLoader.LoadMap();
		cameraController.InitCamera();
		InitArmies();
		DisableGameControllers();
	}

	private void GetImportedControllers() {
		prebattlePhases = new List<PreBattleController>();
		tutorialController = tutorialController != null ? tutorialController : FindObjectOfType(typeof(TutorialController)) as TutorialController;
		scriptedAI = scriptedAI != null ? scriptedAI : FindObjectOfType(typeof(ScriptedAIBattleController)) as ScriptedAIBattleController;
		if (deploymentController) {
			prebattlePhases.Add(deploymentController);
		}
		if (tutorialController) {
			if (tutorialController.isBeforeDeployment) {
				prebattlePhases.Insert(0, tutorialController);
			} else {
				prebattlePhases.Add(tutorialController);
			}
		}
		preBattlePhaseIndex = 0;
	}

	private void InitArmies() {
		player.InitUnits();
		ai.InitUnits();
		if(scriptedAI) {
			scriptedAI.InitUnits(); // must come after player and ai controllers, overwrites list of player controlled units 
		}
	}

	private void StartBattlePhases() {
		Debug.Log("Starting battle map phases");
		if (prebattlePhases.Count > 0) {
			prebattlePhases[0].StartPreBattlePhase();
		} else {
			EnableGameControllers();
		}
	}

	void Update() {
		// move to the next phase
		if (preBattlePhaseIndex < prebattlePhases.Count && !prebattlePhases[preBattlePhaseIndex].enabled) {
			preBattlePhaseIndex++;
			if (preBattlePhaseIndex < prebattlePhases.Count) {
				prebattlePhases[preBattlePhaseIndex].StartPreBattlePhase();
			} else {
				EnableGameControllers();
				player.StartTurn();
			}
		}
	}

	private void EnableGameControllers() {
		player.enabled = true;
		ai.enabled = true;
		battleController.enabled = true;
	}

	private void DisableGameControllers() {
		player.enabled = false;
		ai.enabled = false;
		battleController.enabled = false;
	}
}
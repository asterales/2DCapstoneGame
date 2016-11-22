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
	public HexMap hexMap { get { return mapLoader.battleMap; } }
	public CameraController cameraController;
	public DeploymentController deploymentController;
	public BattleController battleController;
	public PlayerBattleController player;
	public AIBattleController ai;

	// Imported Controllers
	public TutorialController tutorial;
	public ScriptedAIBattleController scriptedAI;
	public CustomUnitLoader unitLoader;
	
	// Prebattle phase management
	public List<PreBattleController> prebattlePhases;
	private int preBattlePhaseIndex;
	

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(this);
		}
	}

	void Start() {
		InitBattleScene();
		StartBattlePhases();
	}

	private void InitBattleScene() {
		GetImportedControllers();
		mapLoader.LoadMap();
		InitArmies();
		List<Vector3> playerPositions = player.units.Where(p => p != null).Select(p => p.transform.position).ToList();
		cameraController.InitCamera(playerPositions);
		InitVictoryCondition();
		DisableGameControllers();
	}

	private void GetImportedControllers() {
		prebattlePhases = new List<PreBattleController>();
		tutorial = tutorial != null ? tutorial : FindObjectOfType(typeof(TutorialController)) as TutorialController;
		scriptedAI = scriptedAI != null ? scriptedAI : FindObjectOfType(typeof(ScriptedAIBattleController)) as ScriptedAIBattleController;
		unitLoader = unitLoader != null ? unitLoader : FindObjectOfType(typeof(CustomUnitLoader)) as CustomUnitLoader;
		if (deploymentController) {
			prebattlePhases.Add(deploymentController);
		}
		if (tutorial) {
			if (tutorial.isBeforeDeployment) {
				prebattlePhases.Insert(0, tutorial);
			} else {
				prebattlePhases.Add(tutorial);
			}
		}
		preBattlePhaseIndex = 0;
	}

	private void InitArmies() {
		player.InitUnits();
		ai.InitUnits();
	}

	private void InitVictoryCondition() {
		if (LevelManager.activeInstance && LevelManager.activeInstance.victoryCondition) {
			battleController.victoryCondition = LevelManager.activeInstance.victoryCondition;
		}
		battleController.victoryCondition.Init();
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
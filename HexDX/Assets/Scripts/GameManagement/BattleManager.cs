using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BattleManager : MonoBehaviour {
	// singleton
	public static BattleManager instance;

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
	
	// Battle phase management
	public List<PhaseController> battlePhases;
	private int phaseIndex;
	

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
	}

	private void GetImportedControllers() {
		tutorial = tutorial != null ? tutorial : FindObjectOfType(typeof(TutorialController)) as TutorialController;
		scriptedAI = scriptedAI != null ? scriptedAI : FindObjectOfType(typeof(ScriptedAIBattleController)) as ScriptedAIBattleController;
		unitLoader = unitLoader != null ? unitLoader : FindObjectOfType(typeof(CustomUnitLoader)) as CustomUnitLoader;
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
		InitPhases();
		Debug.Log("Starting battle map phases");
		if (battlePhases.Count > 0) {
			battlePhases[0].StartBattlePhase();
		}
	}

	private void InitPhases() {
		battlePhases = new List<PhaseController>();
		if (deploymentController) {
			battlePhases.Add(deploymentController);
		}
		if (tutorial) {
			if (tutorial.isBeforeDeployment) {
				battlePhases.Insert(0, tutorial);
			} else {
				battlePhases.Add(tutorial);
			}
		}
		battlePhases.Add(battleController);
		phaseIndex = 0;
	}

	void Update() {
		// move to the next phase after phase script finishes and disables itself
		if (phaseIndex < battlePhases.Count && !battlePhases[phaseIndex].enabled) {
			phaseIndex++;
			if (phaseIndex < battlePhases.Count) {
				battlePhases[phaseIndex].StartBattlePhase();
			}
		}
	}
}
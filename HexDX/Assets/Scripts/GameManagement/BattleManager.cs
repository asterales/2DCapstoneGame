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
	public LevelManager lm;
	
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
		GetImportedComponents();
		LoadMap();
		InitArmies();
		InitVictoryCondition();
		InitCamera();
	}

	private void GetImportedComponents() {
		Debug.Log("BattleManager - Searching for optional components");
		lm = LevelManager.activeInstance;
		tutorial = tutorial != null ? tutorial : FindObjectOfType(typeof(TutorialController)) as TutorialController;
		scriptedAI = scriptedAI != null ? scriptedAI : FindObjectOfType(typeof(ScriptedAIBattleController)) as ScriptedAIBattleController;
		unitLoader = unitLoader != null ? unitLoader : FindObjectOfType(typeof(CustomUnitLoader)) as CustomUnitLoader;
		if (!tutorial) {
			Debug.Log("BattleManager - no TutorialController found");
		}
		if (!scriptedAI) {
			Debug.Log("BattleManager - no ScriptedAIBattleController found");
		}
		if (!unitLoader) {
			Debug.Log("BattleManager - no CustomUnitLoader found");
		}
	}

	private void LoadMap() {
		Debug.Log("BattleManager - Loading map");
		if (lm) {
			mapLoader.csvMapFile = lm.mapFileName;
		}
		mapLoader.LoadMap(unitLoader);
	}

	private void InitArmies() {
		Debug.Log("BattleManager - Initializing player and AI armies");
		player.InitUnits();
		ai.InitUnits();
	}

	private void InitVictoryCondition() {
		if (lm && lm.victoryCondition) {
			battleController.victoryCondition = lm.victoryCondition;
		} else {
			Debug.Log("BattleManager - Using default victory condition - Annhilation");
		}
		Debug.Log("BattleManager - Initializing victory condition - " + battleController.victoryCondition.GetType());
		battleController.victoryCondition.Init();
	}

	private void InitCamera() {
		List<Vector3> playerPositions = player.units.Where(p => p != null).Select(p => p.transform.position).ToList();
		Debug.Log("BattleManager - Initializing camera with " + playerPositions.Count + " player unit positions");
		cameraController.InitCamera(playerPositions);
	}

	private void StartBattlePhases() {
		Debug.Log("BattleManager - Initializing and starting battle map phases");
		InitPhases();
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
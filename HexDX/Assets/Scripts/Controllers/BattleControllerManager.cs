using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BattleControllerManager : MonoBehaviour {
	// singleton
	public static BattleControllerManager instance;

	public BattleController battleController;

	// Prebattle Controllers
	public TutorialController tutorialController;
	public DeploymentController deploymentController;
	public List<PreBattleController> prebattlePhases;
	private int preBattlePhaseIndex;
	
	// Army Battle Controllers
	public PlayerBattleController player;
	public AIBattleController ai;
	public ScriptedAIBattleController scriptedAI;

	void Awake() {
		if (instance == null) {
			instance = this;
			GetDefaultControllers();
			SceneManager.sceneLoaded += OnBattleSceneLoaded;
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

	private void GetDefaultControllers() {
		battleController = battleController != null ? battleController : FindObjectOfType(typeof(BattleController)) as BattleController;
		player = player != null ? player : FindObjectOfType(typeof(PlayerBattleController)) as PlayerBattleController;
		ai = ai != null ? ai : FindObjectOfType(typeof(AIBattleController)) as AIBattleController;
		deploymentController = deploymentController != null ? deploymentController : FindObjectOfType(typeof(DeploymentController)) as DeploymentController;
	}

	void OnDestroy() {
		SceneManager.sceneLoaded -= OnBattleSceneLoaded;
	}

	private void OnBattleSceneLoaded(Scene scene, LoadSceneMode mode) {
		GetImportedControllers();
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

	void Start() {
		StartBattlePhases();
	}

	private void StartBattlePhases() {
		Debug.Log("Starting battle map phases");
		if (prebattlePhases.Count > 0) {
			DisableGameControllers();
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
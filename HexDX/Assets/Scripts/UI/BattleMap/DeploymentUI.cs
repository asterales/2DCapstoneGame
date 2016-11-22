using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeploymentUI : MonoBehaviour {
	public DeploymentController deploymentController;
	private GameDialogueManager gameDialogueMgr;

	private GameObject phasePanelObj;
	private GameObject startBattleButtonObj;
	private Button startBattleButton;
	private bool isActive;

	private bool hoveredStartButton;

	void Awake() {
		gameDialogueMgr = FindObjectOfType(typeof(GameDialogueManager)) as GameDialogueManager;
		phasePanelObj = transform.Find("Title Panel").gameObject;
		startBattleButtonObj = transform.Find("Start Button").gameObject;
		startBattleButton = startBattleButtonObj.GetComponent<Button>();
	}

	void Start() {
		deploymentController = BattleControllerManager.instance.deploymentController;
		startBattleButton.onClick.AddListener(deploymentController.EndPreBattlePhase);
		HideUI();
		if (!deploymentController.enabled) {
			HideUI();
		}
	}

	void Update() {
		if (deploymentController.enabled) {
			if (!isActive) {
				ShowUI();
			}
			if (gameDialogueMgr == null || !gameDialogueMgr.IsVisible) {
				phasePanelObj.SetActive(true);
				startBattleButton.enabled = SelectionController.instance.mode == SelectionMode.DeploymentOpen;
			} else {
				phasePanelObj.SetActive(false);
				startBattleButton.enabled = false;
			}
		} else {
			if (isActive) {
				HideUI();
			}
		}
		Camera.main.GetComponent<CameraController>().enabledMousePan = !hoveredStartButton;
	}

	// attached to button as EventTrigger in inspector, hack to prevent camera from moving
	public void OnStartButtonEnter() {
		hoveredStartButton = true;
	}

	// attached to button as EventTrigger in inspector, hack to prevent camera from moving
	public void OnStartButtonExit() {
		hoveredStartButton = false;
	}

	private void HideUI() {
		isActive = false;
		phasePanelObj.SetActive(false);
		startBattleButtonObj.SetActive(false);
	}

	private void ShowUI() {
		isActive = true;
		phasePanelObj.SetActive(true);
		startBattleButtonObj.SetActive(true);
	}
}
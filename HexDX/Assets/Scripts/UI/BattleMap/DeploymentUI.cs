using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeploymentUI : MonoBehaviour {
	public DeploymentController deploymentController;
	private GameDialogueManager gameDialogueMgr;

	private GameObject phasePanelObj;
	private GameObject startBattleButtonObj;
	private Button startBattleButton;
	private bool isActive;

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
			gameObject.SetActive(false);
		}
	}

	void Update() {
		if (deploymentController.enabled) {
			if (!isActive) {
				ShowUI();
			}
			if (gameDialogueMgr == null || !gameDialogueMgr.IsVisible) {
				phasePanelObj.SetActive(true);
				startBattleButton.enabled = true;
			} else {
				phasePanelObj.SetActive(false);
				startBattleButton.enabled = false;
			}
		} else {
			if (isActive) {
				HideUI();
			}
		}
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
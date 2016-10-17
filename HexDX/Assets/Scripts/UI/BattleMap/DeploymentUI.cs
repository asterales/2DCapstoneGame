using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeploymentUI : MonoBehaviour {
	public DeploymentController deploymentController;
	private GameObject phasePanel;
	private Button startBattleButton;
	private GameDialogueManager gameDialogueMgr;

	void Awake() {
		gameDialogueMgr = FindObjectOfType(typeof(GameDialogueManager)) as GameDialogueManager;
		phasePanel = transform.Find("Title Panel").gameObject;
		startBattleButton = transform.Find("Start Button").GetComponent<Button>();
		startBattleButton.onClick.AddListener(deploymentController.EndPreBattlePhase);
	}

	void Start() {
		if (!deploymentController.enabled) {
			gameObject.SetActive(false);
		}
	}

	void Update() {
		if (gameDialogueMgr == null || !gameDialogueMgr.IsVisible) {
			phasePanel.SetActive(true);
			startBattleButton.enabled = true;
		} else {
			phasePanel.SetActive(false);
			startBattleButton.enabled = false;
		}
	}
}
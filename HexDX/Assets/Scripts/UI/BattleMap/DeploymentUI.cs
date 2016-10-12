using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeploymentUI : MonoBehaviour {
	public DeploymentController deploymentController;
	private Button startBattleButton;

	void Awake() {
		startBattleButton = transform.Find("Start Button").GetComponent<Button>();
		startBattleButton.onClick.AddListener(deploymentController.EndPreBattlePhase);
	}

	void Start() {
		if (!deploymentController.enabled) {
			gameObject.SetActive(false);
		}
	}
}
using UnityEngine;
using UnityEngine.UI;

public class WorldMapMenu : MonoBehaviour {
	private Button trainButton;
	private Button promoteButton;
	private Button upgradeButton;
	private Button quitButton;

	void Awake() {
		trainButton = GameObject.Find("Train").GetComponent<Button>();
		promoteButton = GameObject.Find("Promote").GetComponent<Button>();
		upgradeButton = GameObject.Find("Upgrade").GetComponent<Button>();
		quitButton = GameObject.Find("Quit").GetComponent<Button>();
		if (trainButton == null) {
			Debug.Log("Train Button not found - WorldMapMenu.cs");
		}
		if (promoteButton == null) {
			Debug.Log("Promote Button not found - WorldMapMenu.cs");
		}
		if (upgradeButton == null) {
			Debug.Log("Upgrade Button not found - WorldMapMenu.cs");
		}
		if (quitButton == null) {
			Debug.Log("Quit Button not found - WorldMapMenu.cs");
		}
	}

	void Start() {
		trainButton.onClick.AddListener(Train);
		promoteButton.onClick.AddListener(Promote);
		upgradeButton.onClick.AddListener(Upgrade);
		quitButton.onClick.AddListener(Quit);
	}

	private void Train() {
		Debug.Log("Registered Train Button");
		// implement later
	}

	private void Promote() {
		Debug.Log("Registered Promote Button");
		// implement later
	}

	private void Upgrade() {
		Debug.Log("Registered Upgrade Button");
	}

	private void Quit() {
		Debug.Log("Registered Quit Button");
		Application.Quit();
	}
}
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public GameObject creditsPanel;
	private Button startButton;
	private Button optionsButton;
	private Button creditsButton;
	private Button quitButton;

	void Awake() {
		startButton = transform.Find("Start").GetComponent<Button>();
		//optionsButton = transform.Find("Options").GetComponent<Button>();
		creditsButton = transform.Find("Credits").GetComponent<Button>();
		quitButton = transform.Find("Quit").GetComponent<Button>();
		if (startButton == null) {
			Debug.Log("Start Button not found - MainMenu.cs");
		}
		/*if (optionsButton == null) {
			Debug.Log("Options Button not found - MainMenu.cs");
		}*/
		if (creditsButton == null) {
			Debug.Log("Credits Button not found - MainMenu.cs");
		}
		if (quitButton == null) {
			Debug.Log("Quit Button not found - MainMenu.cs");
		}
	}

	void Start() {
		// startButton listener can be set to LevelLoader.startlevel() in editor
		//optionsButton.onClick.AddListener(Options);
		creditsButton.onClick.AddListener(CreditsOpen);
		Button creditsBackButton = creditsPanel.GetComponentInChildren<Button>();
		creditsBackButton.onClick.AddListener(CreditsClose);
		creditsBackButton.onClick.AddListener(GameManager.instance.PlayCursorSfx);
		quitButton.onClick.AddListener(Quit);
		creditsPanel.SetActive(false);
	}

	private void Options() {
		Debug.Log("Registered Options Button");
		// implement later
	}

	private void CreditsOpen() {
		GameManager.instance.PlayCursorSfx();
		creditsPanel.SetActive(true);
	}

	private void CreditsClose() {
		GameManager.instance.PlayCursorSfx();
		creditsPanel.SetActive(false);
	}

	private void Quit() {
		Debug.Log("Registered Quit Button");
		GameManager.instance.PlayCursorSfx();
		Application.Quit();
	}
}
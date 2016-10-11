using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	private Button startButton;
	private Button optionsButton;
	private Button creditsButton;
	private Button quitButton;

	void Awake() {
		startButton = transform.Find("Start").gameObject.GetComponent<Button>();
		optionsButton = transform.Find("Options").gameObject.GetComponent<Button>();
		creditsButton = transform.Find("Credits").gameObject.GetComponent<Button>();
		quitButton = transform.Find("Quit").gameObject.GetComponent<Button>();
		if (startButton == null) {
			Debug.Log("Start Button not found - MainMenu.cs");
		}
		if (optionsButton == null) {
			Debug.Log("Options Button not found - MainMenu.cs");
		}
		if (creditsButton == null) {
			Debug.Log("Credits Button not found - MainMenu.cs");
		}
		if (quitButton == null) {
			Debug.Log("Quit Button not found - MainMenu.cs");
		}
	}

	void Start() {
		// startButton listener can be set to LevelLoader.startlevel() in editor
		optionsButton.onClick.AddListener(Options);
		creditsButton.onClick.AddListener(Credits);
		quitButton.onClick.AddListener(Quit);
	}

	private void Options() {
		Debug.Log("Registered Options Button");
		// implement later
	}

	private void Credits() {
		Debug.Log("Registered Credits Button");
		// implement later
	}

	private void Quit() {
		Debug.Log("Registered Quit Button");
		Application.Quit();
	}
}
using UnityEngine;
using UnityEngine.UI;

public class KeyPrompt : MonoBehaviour {
	private Image panel;
	private Text promptText;

	public static KeyPrompt instance;

	public string PromptText {
		get { return promptText.text; }
		set { promptText.text = value.ToLower(); } // lowercase if using ringbearer text
	}

	void Awake() {
		if (instance == null) {
			instance = this;
			Init();
		} else {
			Destroy(this);
		}
	}

	private void Init() {
		panel = GetComponent<Image>();
		promptText = transform.Find("Prompt Text").GetComponent<Text>();
		Hide();
	}

	public void Hide() {
		panel.enabled = false;
		promptText.enabled = false;
	}

	public void Show() {
		panel.enabled = true;
		promptText.enabled = true;
	}
}
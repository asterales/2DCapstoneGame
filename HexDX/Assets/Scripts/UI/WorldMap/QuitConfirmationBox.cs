using UnityEngine;
using UnityEngine.UI;

public class QuitConfirmationBox : WorldMapPopupPanel {

	private Button confirmButton;

	protected override void Awake() {
		base.Awake();
		confirmButton = transform.Find("Yes Button").GetComponent<Button>();
		confirmButton.onClick.AddListener(Quit);
	}

	private void Quit() {
		Debug.Log("Registered Quit Button");
		GameManager.instance.PlayCursorSfx();
		Application.Quit();
	}

}
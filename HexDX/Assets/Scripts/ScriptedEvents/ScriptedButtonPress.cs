using UnityEngine;
using UnityEngine.UI;

public class ScriptedButtonPress : ScriptEvent {
	public Button mainButton;
	private bool prevButtonState;

	public override void DoPlayerEvent() {
		SelectionController.instance.mode = SelectionMode.ScriptedButtonPress;
		mainButton.onClick.AddListener(OnClick);
		mainButton.GetComponent<Image>().color = ScriptList.highlightColor;
		prevButtonState = mainButton.enabled;
		mainButton.enabled = true;
	}

	protected virtual void OnClick() {
		mainButton.enabled = prevButtonState;
		mainButton.onClick.RemoveListener(OnClick);
		mainButton.GetComponent<Image>().color = Color.white;
		FinishEvent();
	}

	protected override void EarlyCleanUp() {
		mainButton.enabled = prevButtonState;
		mainButton.onClick.RemoveListener(OnClick);
		mainButton.GetComponent<Image>().color = Color.white;
	}

	public override void DoEvent() {
    	FinishEvent();
    }
}
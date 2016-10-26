using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScriptedRecruit : ScriptEvent {
	// setup to just recruit any unit
	public Button changeArmyPanelButton;
	public Button recruitPanelButton;
	public Button recruitButton;

	public override void DoPlayerEvent() {
		recruitButton.onClick.AddListener(OnClick);
		recruitPanelButton.GetComponent<Image>().color = ScriptList.highlightColor;
		changeArmyPanelButton.interactable = false;
	}

	void Update() {
		if(isActive) {
			if (recruitButton.enabled && recruitButton.interactable) {
				recruitButton.GetComponent<Image>().color = ScriptList.highlightColor;
			} else {
				recruitButton.GetComponent<Image>().color = Color.white;
			}
		}
	}

	private void OnClick() {
		recruitButton.onClick.RemoveListener(OnClick);
		changeArmyPanelButton.interactable = true;
		recruitButton.GetComponent<Image>().color = Color.white;
		recruitPanelButton.GetComponent<Image>().color = Color.white;
		FinishEvent();
	}

	public override void DoEvent() {
		FinishEvent();
	}
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// set up to recruit any single unit
public class ScriptedRecruit : ScriptedButtonPress {
	
	void Update() {
		if(isActive) {
			if (mainButton.enabled && mainButton.interactable) {
				mainButton.GetComponent<Image>().color = ScriptList.highlightColor;
			} else {
				mainButton.GetComponent<Image>().color = Color.white;
			}
		}
	}

	public override void DoEvent() {
    	FinishEvent();
    }
}
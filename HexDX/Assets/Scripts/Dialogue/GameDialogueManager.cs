using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/* 	Manages in game dialogue 
	Currently configured for forced tutorial level
*/

public class GameDialogueManager : DialogueManager {
	private bool isVisible;
	
	void Awake(){
		activeSpeaker = new SpeakerUI("Dialogue Portrait", "Dialogue Name Card", "Dialogue Box");
		HideGUI();
	}

	protected override void Update() {
		if(isVisible) {
			base.Update();
			if (Input.GetMouseButtonDown(0) && !SpeakerLinesFinished()){
				FinishSpeakerLines();
			}
		}
	}

	public void SetLine(string line) {
		if(!SpeakerLinesFinished()){
			FinishSpeakerLines();
		}
		currentLine = line;
		StartSpeakerLines();
	}

	public void SetSpeaker(Character character, Expression expression) {
		activeSpeaker.SetSpeaker(character.Portraits[(int)expression], character.Name);
	}

	public void HideGUI() {
		isVisible = false;
		activeSpeaker.HideGUI();
	}

	public void ShowGUI() {
		isVisible = true;
		activeSpeaker.ShowGUI();
	}
}
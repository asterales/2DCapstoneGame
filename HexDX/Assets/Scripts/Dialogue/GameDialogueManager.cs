using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/* 	Manages in game dialogue 
	Currently configured for forced tutorial level
*/

public class GameDialogueManager : DialogueManager {
	public delegate void FinishedLinesCallback();
	private Queue<string> lines;
	private FinishedLinesCallback finishedCallback;
	public bool IsVisible { get; private set; }
	
	void Awake(){
		activeSpeaker = new SpeakerUI("Dialogue Portrait", "Dialogue Name Card", "Dialogue Box");
		HideGUI();
	}

	protected override void Update() {
		if(IsVisible) {
			if(HasFinishedAllLines() && finishedCallback != null){
				finishedCallback();
				finishedCallback = null;
			}
			base.Update();
		}
	}

	protected override void SetNextLine() {
		if(lines.Count > 0) {
			currentLine = lines.Dequeue();
			StartSpeakerLines();
		}
	}

	public void SetNewLines(List<string> nextLines, FinishedLinesCallback callback = null) {
		if(!SpeakerLinesFinished()){
			FinishSpeakerLines();
		}
		lines = new Queue<string>(nextLines);
		finishedCallback = callback;
		SetNextLine();
	}

	public bool HasFinishedAllLines() {
		return lines.Count == 0 && SpeakerLinesFinished();
	}

	public void SetSpeaker(Character character, int portraitNum) {
		activeSpeaker.SetSpeaker(character.Portraits[portraitNum], character.Name);
	}

	public void HideGUI() {
		IsVisible = false;
		activeSpeaker.HideGUI();
	}

	public void ShowGUI() {
		IsVisible = true;
		activeSpeaker.ShowGUI();
	}
}
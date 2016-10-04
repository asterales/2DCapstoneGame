using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/* 	Manages in game dialogue 
	Currently configured for forced tutorial level
*/

public class GameDialogueManager : DialogueManager {
	public delegate void FinishedLinesCallback();
	public BattleController battleController;
	private bool isVisible;
	private Queue<string> lines;
	private FinishedLinesCallback finishedCallback;
	
	void Awake(){
		activeSpeaker = new SpeakerUI("Dialogue Portrait", "Dialogue Name Card", "Dialogue Box");
	}

	void Start() {
		HideGUI();
	}

	protected override void Update() {
		if(isVisible) {
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

	public void SetSpeaker(Character character, Expression expression) {
		activeSpeaker.SetSpeaker(character.Portraits[(int)expression], character.Name);
	}

	public void HideGUI() {
		isVisible = false;
		activeSpeaker.HideGUI();
		battleController.showTurnBanner = true;
	}

	public void ShowGUI() {
		isVisible = true;
		activeSpeaker.ShowGUI();
		battleController.showTurnBanner = false;
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour {
	public Queue<Dialogue> dialogues;
	private SpeakerUI leftSpeaker;
	private SpeakerUI rightSpeaker;
	private SpeakerUI activeSpeaker;
	private Dialogue currentLine;
	private IEnumerator currentSpeechRoutine;

	void Awake() {
		leftSpeaker = new SpeakerUI("Left Portrait", "Left Name Card", "Left Dialogue Box");
		rightSpeaker = new SpeakerUI("Right Portrait", "Right Name Card", "Right Dialogue Box");
		leftSpeaker.HideAll();
		rightSpeaker.HideAll();
	}

	void Start() {
		SetNextLine();
	}

	private void SetNextLine() {
		if(dialogues.Count > 0) {
			Dialogue dialogue = dialogues.Dequeue();
			switch(dialogue.Side) {
				case ScreenLocation.Left:
					SwitchToSpeaker(leftSpeaker, dialogue);
					break;
				case ScreenLocation.Right:
					SwitchToSpeaker(rightSpeaker, dialogue);
					break;
			}
		}
	}

	private void SwitchToSpeaker(SpeakerUI nextSpeaker, Dialogue dialogue){
		if(activeSpeaker != null) {
			activeSpeaker.HideTextBoxes();
		}
		nextSpeaker.SetSpeaker(dialogue);
		nextSpeaker.ShowPortrait();
		nextSpeaker.ShowTextBoxes();
		activeSpeaker = nextSpeaker;
		currentLine = dialogue;
		StartSpeakerLines();
	}

	private void StartSpeakerLines(){
		if (currentLine.Line != null) {
			currentSpeechRoutine = WriteDialogue();
			StartCoroutine(currentSpeechRoutine);
		}
	}

	public IEnumerator WriteDialogue() {
		activeSpeaker.HideContinuePrompt();
		activeSpeaker.DialogueText = "";
		foreach(char c in currentLine.Line) {
			activeSpeaker.DialogueText += c;
			yield return new WaitForSeconds(0.02f);
		}
	}

	public bool SpeakerLinesFinished() {
		return activeSpeaker.DialogueText.Equals(currentLine.Line);
	}

	public void FinishSpeakerLines() {
		if (currentSpeechRoutine != null) {
			StopCoroutine(currentSpeechRoutine);
		}
		activeSpeaker.DialogueText = currentLine.Line;
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)){
			if(SpeakerLinesFinished()){
				SetNextLine();
			} else {
				FinishSpeakerLines();
			}
		}
		if(SpeakerLinesFinished()){
			activeSpeaker.ShowContinuePrompt();
		} 
	}
}


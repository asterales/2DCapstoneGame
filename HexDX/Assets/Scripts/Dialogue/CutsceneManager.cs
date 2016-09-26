using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour {
	public string csvCutsceneFile = "Assets/Cutscenes/TestDialogue.txt";
	public Queue<Dialogue> dialogues;
	private SpeakerUI leftSpeaker;
	private SpeakerUI rightSpeaker;
	private SpeakerUI activeSpeaker;
	private Dialogue currentLine;
	private IEnumerator currentSpeechRoutine;

	void Awake() {
		if (csvCutsceneFile != null) {
			LoadCutscene(csvCutsceneFile);
		}
		leftSpeaker = new SpeakerUI("Left Portrait", "Left Name Card", "Left Dialogue Box");
		rightSpeaker = new SpeakerUI("Right Portrait", "Right Name Card", "Right Dialogue Box");
		leftSpeaker.HideAll();
		rightSpeaker.HideAll();
	}

	private void LoadCutscene(string file) {
		Debug.Log(file);
		dialogues = new Queue<Dialogue>();
		StreamReader reader = new StreamReader(File.OpenRead(file));
		while(!reader.EndOfStream){
			dialogues.Enqueue(new Dialogue(reader.ReadLine()));
		}
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

	private IEnumerator WriteDialogue() {
		activeSpeaker.HideContinuePrompt();
		activeSpeaker.DialogueText = "";
		foreach(char c in currentLine.Line) {
			activeSpeaker.DialogueText += c;
			yield return new WaitForSeconds(0.02f);
		}
	}

	private bool SpeakerLinesFinished() {
		return activeSpeaker.DialogueText.Equals(currentLine.Line);
	}

	private void FinishSpeakerLines() {
		if (currentSpeechRoutine != null) {
			StopCoroutine(currentSpeechRoutine);
		}
		activeSpeaker.DialogueText = currentLine.Line;
	}

	void Update() {
		if(SpeakerLinesFinished()){
			activeSpeaker.ShowContinuePrompt();
		} 
		if (Input.GetMouseButtonDown(0)){
			if(SpeakerLinesFinished()){
				SetNextLine();
			} else {
				FinishSpeakerLines();
			}
		}
	}
}


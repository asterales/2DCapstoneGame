using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/* Manages cutscene dialogue */

public class CutsceneManager : DialogueManager {
	private static readonly string cutsceneDir = "Cutscenes/";
	public string cutsceneFile;
	private Queue<CutsceneDialogue> dialogues;
	private SpeakerUI leftSpeaker;
	private SpeakerUI rightSpeaker;
	private bool nextSceneLoaded;

	void Awake() {
		if (cutsceneFile != null) {
			LoadCutscene(cutsceneFile);
		}
		leftSpeaker = new SpeakerUI("Left Portrait", "Left Name Card", "Left Dialogue Box");
		rightSpeaker = new SpeakerUI("Right Portrait", "Right Name Card", "Right Dialogue Box");
		leftSpeaker.HideGUI();
		rightSpeaker.HideGUI();
	}

	private void LoadCutscene(string file) {
		dialogues = new Queue<CutsceneDialogue>();
		string[] cutsceneLines = GameResources.GetFileLines(cutsceneDir + file);
		if (cutsceneLines != null) {
			foreach(string line in cutsceneLines) {
				dialogues.Enqueue(new CutsceneDialogue(line));
			}
		} else {
			Debug.Log("Error: cutscene file does not exist: " + cutsceneFile + " - CutsceneManager.cs");
		}
	}

	void Start() {
		SetNextLine();
		nextSceneLoaded = false;
	}

	protected override void Update() {
		if (dialogues.Count == 0 && SpeakerLinesFinished() && Input.GetMouseButtonDown(0) && !nextSceneLoaded) {
			nextSceneLoaded = true; // prevents spam clicks from skipping scenes
			LevelManager.LoadNextScene();
		} else {
			base.Update();
		}
	}

	protected override void SetNextLine() {
		if(dialogues.Count > 0) {
			CutsceneDialogue dialogue = dialogues.Dequeue();
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

	private void SwitchToSpeaker(SpeakerUI nextSpeaker, CutsceneDialogue dialogue){
		if(activeSpeaker != null) {
			activeSpeaker.HideTextBoxes();
		}
		nextSpeaker.SetSpeaker(dialogue.Portrait, dialogue.CharacterName);
		nextSpeaker.ShowGUI();
		activeSpeaker = nextSpeaker;
		currentLine = dialogue.Line;
		StartSpeakerLines();
	}
}


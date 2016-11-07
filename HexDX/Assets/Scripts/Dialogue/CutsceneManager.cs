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
		leftSpeaker = GameObject.Find("Left Speaker").GetComponent<SpeakerUI>();
		rightSpeaker = GameObject.Find("Right Speaker").GetComponent<SpeakerUI>();
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
		leftSpeaker.OverrideSorting = true;
		rightSpeaker.OverrideSorting = true;
		SetNextLine();
		nextSceneLoaded = false;
	}

	protected override void Update() {
		if (dialogues.Count == 0 && SpeakerLinesFinished() && Input.GetMouseButtonDown(0) && !nextSceneLoaded) {
			nextSceneLoaded = true; // prevents spam clicks from skipping scenes
			if (LevelManager.activeInstance) {
				LevelManager.activeInstance.NextScene();
			} else {
				Debug.Log("No active evel manager set");
			}
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
			activeSpeaker.SortingOrder = 1;
		}
		nextSpeaker.SetSpeaker(dialogue.Portrait, dialogue.CharacterName);
		nextSpeaker.ShowGUI();
		activeSpeaker = nextSpeaker;
		activeSpeaker.SortingOrder = 2;
		currentLine = dialogue.Line;
		StartSpeakerLines();
	}
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;

/* Manages cutscene dialogue */

public class CutsceneManager : DialogueManager {
	private static readonly string cutsceneDir = "Cutscenes/";
	private static readonly string backgroundsDir = "backgrounds/";
	private static readonly string musicDir = "Music/";

	public string cutsceneFile;
	public Image bgImage;
	public AudioSource bgm;
	public AudioSource sfx;

	private Queue<CutsceneDialogue> dialogues;
	private SpeakerUI leftSpeaker;
	private SpeakerUI rightSpeaker;
	private bool nextSceneLoaded;

	void Awake() {
		leftSpeaker = GameObject.Find("Left Speaker").GetComponent<SpeakerUI>();
		rightSpeaker = GameObject.Find("Right Speaker").GetComponent<SpeakerUI>();
	}

	void Start() {
		if (LevelManager.activeInstance) {
			cutsceneFile = LevelManager.activeInstance.GetCurrentSceneFile();
		}
		if (cutsceneFile != null && cutsceneFile.Length > 0) {
			LoadCutscene(cutsceneFile);
		}
		leftSpeaker.OverrideSorting = true;
		rightSpeaker.OverrideSorting = true;
		SetNextLine();
		nextSceneLoaded = false;
	}

	private void LoadCutscene(string file) {
		dialogues = new Queue<CutsceneDialogue>();
		string[] cutsceneLines = GameResources.GetFileLines(cutsceneDir + file);
		if (cutsceneLines != null) {
			string bgLine = cutsceneLines[0];
			int startIndex = Convert.ToInt32(ParseBackgroundAssets(bgLine));
			for(int i = startIndex; i < cutsceneLines.Length; i++) {
				dialogues.Enqueue(new CutsceneDialogue(cutsceneLines[i]));
			}
		} else {
			Debug.Log("Error: cutscene file does not exist: " + cutsceneFile + " - CutsceneManager.cs");
		}
	}

	private bool ParseBackgroundAssets(string bgLine) {
		if (!bgLine.Contains("|")) {
			string[] bgTokens = bgLine.Split(',').Select(s => s.Trim()).ToArray();
			bgImage.sprite = Resources.Load<Sprite>(backgroundsDir + bgTokens[0]);
			bgImage.color = Color.white;
			if (bgTokens.Length > 1) {
				bgm.clip = Resources.Load<AudioClip>(musicDir + bgTokens[1]);
				bgm.Play();
			}
			return true;
		}
		return false;
	}

	protected override void Update() {
		if (dialogues.Count == 0 && SpeakerLinesFinished() && Input.GetMouseButtonDown(0) && !nextSceneLoaded) {
			nextSceneLoaded = true; // prevents spam clicks from skipping scenes
			if (LevelManager.activeInstance) {
				LevelManager.activeInstance.NextScene();
			} else {
				Debug.Log("No active level manager set");
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


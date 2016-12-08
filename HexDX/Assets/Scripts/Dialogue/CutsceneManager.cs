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
	private static readonly string sfxDir = "SoundEffects/";

	public string cutsceneFile;
	public Image bgImage;
	public AudioSource bgm;
	public AudioSource sfx;

	private Queue<CutsceneDialogue> dialogues;
	private SpeakerUI leftSpeaker;
	private SpeakerUI rightSpeaker;
	private bool nextSceneLoaded;
	private int moneyGained;
	private int unitsLost;

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
		if (cutsceneLines != null && cutsceneLines.Length > 0) {
			string bgLine = cutsceneLines[0];
			string sideEffectsLine = cutsceneLines[cutsceneLines.Length - 1 ];
			int startIndex = Convert.ToInt32(ParseBackgroundAssets(bgLine));
			int endIndex = cutsceneLines.Length;
			if (!sideEffectsLine.Equals(bgLine)) {
				endIndex = endIndex - Convert.ToInt32(ParseSideEffects(sideEffectsLine));
			}
			for(int i = startIndex; i < endIndex; i++) {
				dialogues.Enqueue(new CutsceneDialogue(cutsceneLines[i]));
			}
		} else {
			Debug.Log("Error: cutscene file does not exist or is empty: " + cutsceneFile + " - CutsceneManager.cs");
		}
	}

	private bool ParseBackgroundAssets(string bgLine) {
		if (!bgLine.Contains("|") && bgLine.Contains(",")) { // Not a dialogue line nor sfx
			string[] bgTokens = bgLine.Split(',').Select(s => s.Trim()).ToArray();
			if (bgTokens[0].Length > 1) {
				bgImage.sprite = Resources.Load<Sprite>(backgroundsDir + bgTokens[0]);
				bgImage.color = Color.white;
			}
			if (bgTokens[1].Length > 1) {
				bgm.clip = Resources.Load<AudioClip>(musicDir + bgTokens[1]);
				bgm.Play();
			}
			return true;
		}
		return false;
	}

	private bool ParseSideEffects(string sideEffectsLine) {
		if (!sideEffectsLine.Contains("|") && sideEffectsLine.Contains(",")) {  // Not a dialogue line nor sfx
			string[] tokens = sideEffectsLine.Split(',').Select(s => s.Trim()).ToArray();
			if (tokens[0].Length > 0) {
				moneyGained = Convert.ToInt32(tokens[0]);
			}
			if (tokens[1].Length > 0) {
				int unitsLostToken = Convert.ToInt32(tokens[1]);
				unitsLost = unitsLostToken > 0 ? unitsLostToken : 0;
			}
			return true;
		}
		return false;
	}

	protected override void Update() {
		if (dialogues.Count == 0 && SpeakerLinesFinished() && !sfx.isPlaying && Input.GetMouseButtonDown(0) && !nextSceneLoaded) {
			nextSceneLoaded = true; // prevents spam clicks from skipping scenes
			ApplySideEffects();
			if (LevelManager.activeInstance) {
				LevelManager.activeInstance.NextScene();
			} else {
				Debug.Log("No active level manager set");
			}
		} else {
			base.Update();
		}
	}

	private void ApplySideEffects() {
		GameManager gm = GameManager.instance;
		if (gm) {
			gm.funds += moneyGained;
			unitsLost = (int)Mathf.Min(unitsLost, gm.playerAllUnits.Count);
			if (unitsLost > 0) {
				List<Unit> unitsByStrength = gm.playerAllUnits.OrderBy(p => p.Health).OrderBy(p => p.Veterancy).ToList();
				for(int i = 0; i < unitsLost; i++) {
					Unit unit = unitsByStrength[i];
					gm.activeUnits.Remove(unit);
					gm.playerAllUnits.Remove(unit);
					unit.transform.parent = null;
				}
			}
		}
	}

	protected override void SetNextLine() {
		if(dialogues.Count > 0 && !sfx.isPlaying) {
			CutsceneDialogue dialogue = dialogues.Dequeue();
			if (dialogue.SfxOnly) {
				PlaySfx(dialogue);
			} else {
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
		if (dialogue.SoundFile != null) {
			PlaySfx(dialogue);
		}
		StartSpeakerLines();
	}

	private void PlaySfx(CutsceneDialogue dialogue) {
		sfx.clip = Resources.Load<AudioClip>(sfxDir + dialogue.SoundFile);
		sfx.Play();
	}
}


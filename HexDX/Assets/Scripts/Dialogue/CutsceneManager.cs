using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

/* Manages cutscene dialogue */

public class CutsceneManager : DialogueManager {
	public string csvCutsceneFile = "Assets/Cutscenes/TestDialogue.txt";
	private Queue<CutsceneDialogue> dialogues;
	private SpeakerUI leftSpeaker;
	private SpeakerUI rightSpeaker;

	void Awake() {
		if (csvCutsceneFile != null) {
			LoadCutscene(csvCutsceneFile);
		}
		leftSpeaker = new SpeakerUI("Left Portrait", "Left Name Card", "Left Dialogue Box");
		rightSpeaker = new SpeakerUI("Right Portrait", "Right Name Card", "Right Dialogue Box");
		leftSpeaker.HideGUI();
		rightSpeaker.HideGUI();
	}

	private void LoadCutscene(string file) {
		dialogues = new Queue<CutsceneDialogue>();
		StreamReader reader = new StreamReader(File.OpenRead(file));
		while(!reader.EndOfStream){
			dialogues.Enqueue(new CutsceneDialogue(reader.ReadLine()));
		}
	}

	void Start() {
		SetNextLine();
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


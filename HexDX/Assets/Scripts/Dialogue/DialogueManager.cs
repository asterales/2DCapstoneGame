using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using System.IO;
using UnityEngine.UI;

/* Base class for dialogue managers */

public abstract class DialogueManager : MonoBehaviour {
	public bool hidePromptOnFinish;
	protected SpeakerUI activeSpeaker;
	protected string currentLine;
	protected IEnumerator<float> currentSpeechRoutine;
    protected int counter = 5; // used to avoid bug where text immediatly appears due to previous event click

	protected virtual void Update() {
        counter--;
		if(SpeakerLinesFinished()){
			if (hidePromptOnFinish) {
				activeSpeaker.HideContinuePrompt();
			} else {
				activeSpeaker.ShowContinuePrompt();
			}
		}
		if (Input.GetMouseButtonDown(0) && counter <= 0){
			if(SpeakerLinesFinished()){
				SetNextLine();
                counter = 5;
			} else {
                counter = 5;
				FinishSpeakerLines();
			}
		} 
	}

	protected abstract void SetNextLine();

	protected void StartSpeakerLines(){
		if (currentLine != null) {
            counter = 5;
			currentSpeechRoutine = WriteDialogue();
			Timing.RunCoroutine(currentSpeechRoutine);
		}
	}

	protected IEnumerator<float> WriteDialogue() {
		activeSpeaker.HideContinuePrompt();
		activeSpeaker.DialogueText = "";
		currentLine = currentLine.Trim();
        string[] words = currentLine.Split(' ');
		foreach(string word in words) {
			activeSpeaker.DialogueText += word + " ";
			yield return Timing.WaitForSeconds(0.02f);
		}
		activeSpeaker.DialogueText = activeSpeaker.DialogueText.Trim();
	}

	public bool SpeakerLinesFinished() {
		if (activeSpeaker == null) {
			return false;
		}
		return activeSpeaker.DialogueText.Equals(currentLine);
	}

	protected void FinishSpeakerLines() {
		if (currentSpeechRoutine != null) {
			Timing.KillCoroutines(currentSpeechRoutine);
		}
		activeSpeaker.DialogueText = currentLine;
	}
}


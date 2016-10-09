using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

/* Base class for dialogue managers */

public abstract class DialogueManager : MonoBehaviour {
	protected SpeakerUI activeSpeaker;
	protected string currentLine;
	protected IEnumerator currentSpeechRoutine;
    protected int counter = 5; // used to avoid bug where text immediatly appears due to previous event click

	protected virtual void Update() {
        counter--;
		if(SpeakerLinesFinished()){
			activeSpeaker.ShowContinuePrompt();
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
			StartCoroutine(currentSpeechRoutine);
		}
	}

	protected IEnumerator WriteDialogue() {
		activeSpeaker.HideContinuePrompt();
		activeSpeaker.DialogueText = "";
        string[] words = currentLine.Split(' ');
		foreach(string word in words) {
			activeSpeaker.DialogueText += word + " ";
			yield return new WaitForSeconds(0.02f);
		}
	}

	public bool SpeakerLinesFinished() {
		if (activeSpeaker == null) {
			return false;
		}
		return activeSpeaker.DialogueText.Equals(currentLine);
	}

	protected void FinishSpeakerLines() {
		if (currentSpeechRoutine != null) {
			StopCoroutine(currentSpeechRoutine);
		}
		activeSpeaker.DialogueText = currentLine;
	}
}


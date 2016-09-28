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
    protected int counter = 10; // used to avoid bug where text immediatly appears due to previous event click

	protected virtual void Update() {
        counter--;
		if(SpeakerLinesFinished()){
			activeSpeaker.ShowContinuePrompt();
		}
		if (Input.GetMouseButtonDown(0) && counter <= 0){
			if(SpeakerLinesFinished()){
				SetNextLine();
                counter = 10;
			} else {
                counter = 10;
				FinishSpeakerLines();
			}
		} 
	}

	protected abstract void SetNextLine();

	protected void StartSpeakerLines(){
		if (currentLine != null) {
            counter = 10;
			currentSpeechRoutine = WriteDialogue();
			StartCoroutine(currentSpeechRoutine);
		}
	}

	protected IEnumerator WriteDialogue() {
		activeSpeaker.HideContinuePrompt();
		activeSpeaker.DialogueText = "";
		foreach(char c in currentLine) {
			activeSpeaker.DialogueText += c;
			yield return new WaitForSeconds(0.02f);
		}
	}

	public bool SpeakerLinesFinished() {
		return activeSpeaker.DialogueText.Equals(currentLine);
	}

	protected void FinishSpeakerLines() {
		if (currentSpeechRoutine != null) {
			StopCoroutine(currentSpeechRoutine);
		}
		activeSpeaker.DialogueText = currentLine;
	}
}


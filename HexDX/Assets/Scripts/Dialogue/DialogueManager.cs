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

	protected virtual void Update() {
		if(SpeakerLinesFinished()){
			activeSpeaker.ShowContinuePrompt();
		} 
	}

	protected void StartSpeakerLines(){
		if (currentLine != null) {
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


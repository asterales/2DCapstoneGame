using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour {
	public Queue<Dialogue> dialogues;

	/* For managing speaker GUI elements */
	private class Speaker {
		private Image portrait;
		private Image nameCard;
		private Text nameText;

		public Speaker(string portraitObjName, string nameCardObjName) {
			portrait = GameObject.Find(portraitObjName).GetComponent<Image>();
			nameCard = GameObject.Find(nameCardObjName).GetComponent<Image>();
			nameText = nameCard.transform.Find("Text").GetComponent<Text>();
		}

		public void ShowNameCard() {
			nameCard.enabled = true;
			nameText.enabled = true;
		}

		public void HideNameCard() {
			nameCard.enabled = false;
			nameText.enabled = false;
		}

		public void SetSpeaker(Dialogue line) {
			portrait.sprite = line.Portrait;
			nameText.text = line.CharacterName;
		}

		public void ShowSpeaker(){
			portrait.enabled = true;
		}

		public void HideSpeaker() {
			portrait.enabled = false;
		}

		public void Hide() {
			HideSpeaker();
			HideNameCard();
		}
	}

	private Text textBox;
	private Speaker leftSpeaker;
	private Speaker rightSpeaker;
	private Speaker activeSpeaker;

	void Awake() {
		textBox = GameObject.Find("Text Box").GetComponent<Text>();
		leftSpeaker = new Speaker("Left Speaker", "Left Name Card");
		rightSpeaker = new Speaker("Right Speaker", "Right Name Card");
		leftSpeaker.Hide();
		rightSpeaker.Hide();
	}

	void Start() {
		SetNextLine();
	}

	private void SetNextLine() {
		if(dialogues.Count > 0) {
			Dialogue nextLine = dialogues.Dequeue();
			SetSpeaker(nextLine);
			SetText(nextLine);
		}
	}

	private void SetSpeaker(Dialogue dialogue){
		switch(dialogue.Side) {
			case ScreenLocation.Left:
				leftSpeaker.SetSpeaker(dialogue);
				leftSpeaker.ShowSpeaker();
				SetActiveNameCard(leftSpeaker);
				break;
			case ScreenLocation.Right:
				rightSpeaker.SetSpeaker(dialogue);
				rightSpeaker.ShowSpeaker();
				SetActiveNameCard(rightSpeaker);
				break;
		}
	}

	private void SetActiveNameCard(Speaker nextSpeaker){
		if (activeSpeaker != null) {
			activeSpeaker.HideNameCard();
		}
		activeSpeaker = nextSpeaker;
		nextSpeaker.ShowNameCard();
	}

	private void SetText(Dialogue dialogue) {
		if (dialogue.Line != null) {
			textBox.text = dialogue.Line;
		}
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)){
			SetNextLine();
		}
	}
}


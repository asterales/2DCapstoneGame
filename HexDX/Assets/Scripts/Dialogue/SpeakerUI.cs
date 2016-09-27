using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/* 	Wrapper class for character dialogue UI elements
	UI elements include: Portrait, namecard, dialogue textbox */

public class SpeakerUI {

	private class TextPanel {
		public Image panelBG;
		public Text textbox;

		public TextPanel(string panelObjName) {
			panelBG = GameObject.Find(panelObjName).GetComponent<Image>();
			textbox = panelBG.transform.Find("Text").GetComponent<Text>();
		}

		public void Hide() {
			panelBG.enabled = false;
			textbox.enabled = false;
		}

		public void Show() {
			panelBG.enabled = true;
			textbox.enabled = true;
		}
	}

	/* UI Elements */
	private Image portrait;
	private TextPanel nameCard;
	private TextPanel dialogueBox;
	private Text continuePrompt;

	public string DialogueText {
		get { return dialogueBox.textbox.text; }
		set { dialogueBox.textbox.text = value; }
	}

	public SpeakerUI(string portraitObjName, string nameCardObjName, string dialogueBoxObjName) {
		portrait = GameObject.Find(portraitObjName).GetComponent<Image>();
		nameCard = new TextPanel(nameCardObjName);
		dialogueBox = new TextPanel(dialogueBoxObjName);
		continuePrompt = dialogueBox.panelBG.transform.Find("Continuation Prompt").GetComponent<Text>();
	}

	public void SetSpeaker(Sprite picture, string name) {
		portrait.sprite = picture;
		nameCard.textbox.text = name;
	}

	public void ShowPortrait(){
		portrait.enabled = true;
	}

	public void HidePortrait() {
		portrait.enabled = false;
	}

	public void ShowTextBoxes() {
		nameCard.Show();
		dialogueBox.Show();
	}

	public void HideTextBoxes() {
		nameCard.Hide();
		dialogueBox.Hide();
		HideContinuePrompt();
	}

	public void ShowContinuePrompt() {
		continuePrompt.enabled = true;
	}

	public void HideContinuePrompt() {
		continuePrompt.enabled = false;
	}

	public void ShowGUI() {
		ShowPortrait();
		ShowTextBoxes();
	}

	public void HideGUI() {
		HidePortrait();
		HideTextBoxes();
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/* 	Wrapper for character dialogue UI elements
	UI elements include: Portrait, namecard, dialogue textbox */

public class SpeakerUI : MonoBehaviour {

	private class TextPanel {
		public Image panelBG;
		public Text textbox;

		public TextPanel(Transform panelTransform) {
			panelBG = panelTransform.GetComponent<Image>();
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
	private Canvas canvas;
	public bool enableSortingOptions;

	public string DialogueText {
		get { return dialogueBox.textbox.text; }
		set { dialogueBox.textbox.text = value; }
	}

	public bool OverrideSorting {
		get { return canvas.overrideSorting; }
		set { canvas.overrideSorting = value; }
	}

	public int SortingOrder {
		get { return canvas.sortingOrder; }
		set { canvas.sortingOrder = value; }
	}

	void Awake() {
		if (enableSortingOptions) {
			canvas = GetComponent<Canvas>();
			if (!canvas) {
				canvas = gameObject.AddComponent<Canvas>();
			}
		}
		portrait = transform.Find("Portrait").GetComponent<Image>();
		nameCard = new TextPanel(transform.Find("Name Card"));
		dialogueBox = new TextPanel(transform.Find("Dialogue Box"));
		continuePrompt = dialogueBox.panelBG.transform.Find("Continuation Prompt").GetComponent<Text>();
		HideGUI();
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
		if (portrait != null) {
			ShowPortrait();
		}
		ShowTextBoxes();
	}

	public void HideGUI() {
		HidePortrait();
		HideTextBoxes();
	}
}
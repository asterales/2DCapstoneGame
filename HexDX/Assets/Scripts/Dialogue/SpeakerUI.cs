using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/* 	Wrapper for character dialogue UI elements
	UI elements include: Portrait, namecard, dialogue textbox */

public class SpeakerUI : MonoBehaviour {

	private class TextPanel {
		public GameObject obj;
		public Text textbox;

		public TextPanel(GameObject panelObj) {
			obj = panelObj;
			textbox = panelObj.transform.Find("Text").GetComponent<Text>();
		}

		public void Hide() {
			obj.SetActive(false);
		}

		public void Show() {
			obj.SetActive(true);
		}
	}

	/* UI Elements */
	public Image portrait;
	public GameObject portraitObj;
	public GameObject namePanelObj;
	public GameObject dialoguePanelObj;
	public Text continuePrompt;
	
	private TextPanel nameCard;
	private TextPanel dialogueBox;
	private Canvas canvas;

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
		canvas = GetComponent<Canvas>();
		if (!canvas) {
			canvas = gameObject.AddComponent<Canvas>();
		}
		nameCard = new TextPanel(namePanelObj);
		dialogueBox = new TextPanel(dialoguePanelObj);
		if (!continuePrompt) {
			continuePrompt = dialoguePanelObj.transform.Find("Continuation Prompt").GetComponent<Text>();
		}
		HideGUI();
	}

	public void SetSpeaker(Sprite picture, string name) {
		if (picture != null) {
			portrait.color = Color.white;
			portrait.sprite = picture;
		} else {
			portrait.color = Color.clear;
		}
		nameCard.textbox.text = name;
	}

	public void ShowPortrait(){
		portraitObj.SetActive(true);
	}

	public void HidePortrait() {
		portraitObj.SetActive(false);
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
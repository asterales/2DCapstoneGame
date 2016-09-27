using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialController : MonoBehaviour {
	private ScriptList eventsList;
	public static GameObject selectionPromptObj;
	public Sprite selectionSprite;
	public RuntimeAnimatorController animation;
	public static Tile targetTile;
	private static readonly Vector3 visibilityOffset = new Vector3(0, 0, -0.01f);

	void Awake(){
		Character tutorialAdvisor = Character.characters[2]; // Colonel Schmidt
		eventsList = GameObject.Find("ScriptedEvents").GetComponent<ScriptList>();
		eventsList.dialogueMgr.SetSpeaker(tutorialAdvisor, Expression.Neutral);
		InitSelectionPrompt();
	}

	private void InitSelectionPrompt() {
		selectionPromptObj = new GameObject(string.Format("Selection Prompt"));
        SpriteRenderer sr = selectionPromptObj.AddComponent<SpriteRenderer>();
        sr.sprite = selectionSprite;
        sr.sortingOrder = 2;
        sr.color = Color.cyan;
        Animator animator = selectionPromptObj.AddComponent<Animator>();
        animator.runtimeAnimatorController = animation;
        selectionPromptObj.transform.position = new Vector3(-1000, -1000, 0);
	}

	public static void ShowSelectionPrompt(Tile tile) {
		selectionPromptObj.transform.position = tile.transform.position + visibilityOffset;
	}

	public static void HideSelectionPrompt() {
		selectionPromptObj.transform.position = new Vector3(-1000, -1000, 0);
	}

	public static bool IsTargetTile(Tile tile){
		return SelectionController.mode == SelectionMode.ScriptedPlayerSelection && tile == targetTile;
	}
	
	void Update () {
		if (eventsList.EventsCompleted){
			EndTutorialMode();
		}
	}

	private void EndTutorialMode() {
		eventsList.dialogueMgr.HideGUI();
		SelectionController.mode = SelectionMode.Open;
		HideSelectionPrompt();
		targetTile = null;
		this.enabled = false;
	}
}

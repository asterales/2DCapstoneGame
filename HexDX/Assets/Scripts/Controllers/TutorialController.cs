﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialController : MonoBehaviour {
	public Sprite selectionSprite;
	public RuntimeAnimatorController animation;
	public PlayerBattleController player;
    public AIBattleController ai;

	public static Tile targetTile;
	public static ScriptList eventsList;
	public static GameObject selectionPromptObj;
	private static readonly Vector3 visibilityOffset = new Vector3(0, 0, -0.01f);

	void Awake(){
		Character tutorialAdvisor = Character.characters[2]; // Colonel Schmidt
		eventsList = GameObject.Find("ScriptedEvents").GetComponent<ScriptList>();
		eventsList.dialogueMgr.SetSpeaker(tutorialAdvisor, 7);
		InitSelectionPrompt();
	}

	void Start() {
		DisableGameControllers();
		eventsList.StartEvents();
	}

	private void InitSelectionPrompt() {
		selectionPromptObj = new GameObject(string.Format("Selection Prompt"));
        SpriteRenderer sr = selectionPromptObj.AddComponent<SpriteRenderer>();
        sr.sprite = selectionSprite;
        sr.sortingOrder = 2;
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

	public static bool IsTargetDestination(MovementTile mvtTile){
		return SelectionController.mode == SelectionMode.ScriptedPlayerMove && mvtTile.tile == targetTile;
	}

	public static bool IsAttackTarget(AttackTile attackTile) {
		return SelectionController.mode == SelectionMode.ScriptedPlayerAttack && attackTile.tile == targetTile;
	}

	public static void EndCurrentTurn(){
		if(eventsList.currentScriptEvent.GetType() != typeof(ScriptedEndTurn)){
			Debug.Log("Error: called EndCurrentTurn when currently not EndTurnScript");
		}
		eventsList.currentScriptEvent.FinishEvent();
	}
	
	void Update () {
		if (targetTile != null) {
			ShowSelectionPrompt(targetTile);
		} else {
			HideSelectionPrompt();
		}
		if (eventsList.EventsCompleted){
			if(eventsList.dialogueMgr.HasFinishedAllLines()) {
				EndTutorialMode();
			}
		}
	}

	private void DisableGameControllers() {
		player.enabled = false;
		ai.enabled = false;
	}

	private void EnableGameControllers() {
		player.enabled = true;
		ai.enabled = true;
	}

	private void EndTutorialMode() {
		eventsList.dialogueMgr.HideGUI();
		HideSelectionPrompt();
		targetTile = null;
		SelectionController.ClearAllSelections();
		EnableGameControllers();
		player.StartTurn();
		this.enabled = false;
	}
}

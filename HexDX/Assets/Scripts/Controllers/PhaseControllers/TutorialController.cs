﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TutorialController : PhaseController {
	public bool isBeforeDeployment;
	private TutorialInfo info;
	private ScriptedAIBattleController scriptedAI;
	private CustomUnitLoader unitLoader;
	
	public Sprite selectionSprite;
	public RuntimeAnimatorController animation;

	public Tile targetTile;
	public ScriptList eventsList;
	public GameObject selectionPromptObj;

	protected override void Awake() {
		info = GetComponent<TutorialInfo>();
		scriptedAI = GetComponent<ScriptedAIBattleController>();
		unitLoader = GetComponent<CustomUnitLoader>();
		eventsList = GetComponent<ScriptList>();
		base.Awake();
	}

	public override void StartBattlePhase() {
		if (!info.HasBeenCompleted()) {
			base.StartBattlePhase();
			if (scriptedAI) {
				scriptedAI.InitUnits();
			}
			targetTile = null;
			InitSelectionPrompt();
			if(!eventsList.dialogueMgr) {
				eventsList.dialogueMgr = FindObjectOfType(typeof(GameDialogueManager)) as GameDialogueManager;
			}
			eventsList.dialogueMgr.SetSpeaker(info.tutorialAdvisor, info.advisorPortraitIndex);
			eventsList.StartEvents();
		} else {
			base.EndBattlePhase();
		}
	}

	private void InitSelectionPrompt() {
		selectionPromptObj = new GameObject(string.Format("Selection Prompt"));
        SpriteRenderer sr = selectionPromptObj.AddComponent<SpriteRenderer>();
        sr.sprite = selectionSprite;
        sr.sortingOrder = 1;
        Animator animator = selectionPromptObj.AddComponent<Animator>();
        animator.runtimeAnimatorController = animation;
        selectionPromptObj.transform.position = GameResources.hidingPosition;
	}

	public void ShowSelectionPrompt(Tile tile) {
		selectionPromptObj.transform.position = tile.transform.position + GameResources.visibilityOffset + new Vector3(0, 0, 0.1f);
	}

	public void HideSelectionPrompt() {
		selectionPromptObj.transform.position = GameResources.hidingPosition;
	}

	public bool IsTargetTile(Tile tile){
		return sc.mode == SelectionMode.ScriptedPlayerSelection && tile == targetTile;
	}

	public bool IsTargetDestination(MovementTile mvtTile){
		return sc.mode == SelectionMode.ScriptedPlayerMove && mvtTile.tile == targetTile;
	}

	public bool IsAttackTarget(AttackTile attackTile) {
		return sc.mode == SelectionMode.ScriptedPlayerAttack && attackTile.tile == targetTile;
	}

	public void EndCurrentTurn(){
		if(eventsList.currentScriptEvent.GetType() != typeof(ScriptedEndTurn)){
			Debug.Log("Error: called EndCurrentTurn when currently not EndTurnScript");
		}
		eventsList.currentScriptEvent.FinishEvent();
	}
	
	void Update() {
		if (targetTile != null) {
			ShowSelectionPrompt(targetTile);
		} else {
			HideSelectionPrompt();
		}
		if (eventsList.IsFinished()){
			EndBattlePhase();
		}
	}

	public override void EndBattlePhase() {
		eventsList.dialogueMgr.HideGUI();
		HideSelectionPrompt();
		targetTile = null;
		info.RegisterCompleted();
		base.EndBattlePhase();
	}
}

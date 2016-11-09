using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TutorialController : PreBattleController {
	public bool isBeforeDeployment;
	public TutorialInfo info;
	private ScriptedAIBattleController scriptedAI;
	public CustomUnitLoader unitLoader;
	
	public Sprite selectionSprite;
	public RuntimeAnimatorController animation;

	public static Tile targetTile;
	public static ScriptList eventsList;
	public static GameObject selectionPromptObj;

	protected override void Awake() {
		base.Awake();
		info = GetComponent<TutorialInfo>();
		scriptedAI = GetComponent<ScriptedAIBattleController>();
		unitLoader = GetComponent<CustomUnitLoader>();
	}

	void OnDestroy() {
		targetTile = null;
	}

	public override void StartPreBattlePhase() {
		if (!info.HasBeenCompleted()) {
			base.StartPreBattlePhase();
			if (scriptedAI) {
				scriptedAI.InitUnits();
			}
			targetTile = null;
			InitSelectionPrompt();
			eventsList = GameObject.Find("ScriptedEvents").GetComponent<ScriptList>();
			if(!eventsList.dialogueMgr) {
				eventsList.dialogueMgr = FindObjectOfType(typeof(GameDialogueManager)) as GameDialogueManager;
			}
			eventsList.dialogueMgr.SetSpeaker(info.tutorialAdvisor, info.advisorPortraitIndex);
			eventsList.StartEvents();
		} else {
			base.EndPreBattlePhase();
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

	public static void ShowSelectionPrompt(Tile tile) {
		selectionPromptObj.transform.position = tile.transform.position + GameResources.visibilityOffset + new Vector3(0, 0, 0.1f);
	}

	public static void HideSelectionPrompt() {
		selectionPromptObj.transform.position = GameResources.hidingPosition;
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
	
	protected override void PhaseUpdateAction() {
		if (targetTile != null) {
			ShowSelectionPrompt(targetTile);
		} else {
			HideSelectionPrompt();
		}
		if (eventsList.EventsCompleted){
			if(eventsList.dialogueMgr.HasFinishedAllLines()) {
				EndPreBattlePhase();
			}
		}
	}

	public override void EndPreBattlePhase() {
		eventsList.dialogueMgr.HideGUI();
		HideSelectionPrompt();
		targetTile = null;
		info.RegisterCompleted();
		base.EndPreBattlePhase();
	}

}

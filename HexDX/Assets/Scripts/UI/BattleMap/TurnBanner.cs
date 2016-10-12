using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TurnBanner : MonoBehaviour {
	private Image playerTurnBanner;
	private Image enemyTurnBanner;
	public BattleController battleController;
	private GameDialogueManager gameDialogueMgr;

	void Awake() {
		gameDialogueMgr = FindObjectOfType(typeof(GameDialogueManager)) as GameDialogueManager;
		playerTurnBanner = transform.Find("PlayerTurn").GetComponent<Image>();
		enemyTurnBanner = transform.Find("EnemyTurn").GetComponent<Image>();
	}

	void Update() {
		if (battleController.enabled 
				&& (gameDialogueMgr == null || !gameDialogueMgr.IsVisible)) {
           if (battleController.IsPlayerTurn){
               ShowPlayerTurn();
           } else {
               ShowEnemyTurn();
           }
		} else {
		   Hide();
		}
	}

	private void ShowPlayerTurn() {
		playerTurnBanner.enabled = true;
        enemyTurnBanner.enabled = false;
	}

	private void ShowEnemyTurn() {
		playerTurnBanner.enabled = false;
        enemyTurnBanner.enabled = true;
	}

	private void Hide() {
		playerTurnBanner.enabled = false;
        enemyTurnBanner.enabled = false;
	}
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TurnBanner : MonoBehaviour {
	public Image playerTurnBanner;
	public Image enemyTurnBanner;
	public BattleController battleController;
	public GameDialogueManager gameDialogueMgr;

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
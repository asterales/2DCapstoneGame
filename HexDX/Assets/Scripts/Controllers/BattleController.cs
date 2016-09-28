using UnityEngine;
using System.Collections.Generic;

// This class will be responsible for handling Game Loop States

public class BattleController : MonoBehaviour {
    public AIBattleController ai;
    public PlayerBattleController player;
    public ScriptedAIBattleController scriptedAI;
    public Texture2D actionMenuItem;
    public Texture2D actionMenuItemHover;
    private bool isPlayerTurn;

    void Awake () {
        PlayerBattleController.menuItem = actionMenuItem;
        PlayerBattleController.menuItemHovered = actionMenuItemHover;
        ai = this.gameObject.GetComponent<AIBattleController>();
        player = this.gameObject.GetComponent<PlayerBattleController>();
        scriptedAI = this.gameObject.GetComponent<ScriptedAIBattleController>();
        ////// DEBUG CODE //////
        if (ai == null && scriptedAI == null)
        {
            Debug.Log("Error :: AI Battle Controller not defined -> BattleController.cs");
        }
        if (player == null)
        {
            Debug.Log("Error :: Player Battle Controller not defined -> BattleController.cs");
        }
        ////////////////////////
    }

    void Start() {
        isPlayerTurn = true;
    }

    public void EndCurrentTurn() {
        if (isPlayerTurn) {
            //ai.EndTurn();
            player.EndTurn();
            isPlayerTurn = false;
            ai.StartTurn();
            Debug.Log("BattleController - Starting AI Turn");
        } else {
            //player.EndTurn();
            ai.EndTurn();
            player.StartTurn();
            isPlayerTurn = true;
            Debug.Log("BattleController - Starting Player Turn");
        }
    }

}

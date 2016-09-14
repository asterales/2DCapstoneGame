using UnityEngine;
using System.Collections.Generic;

// This class will be responsible for handling Game Loop States

public class BattleController : MonoBehaviour {
    public static AIBattleController ai;
    public static PlayerBattleController player;
    public Texture2D actionMenuItem;
    public Texture2D actionMenuItemHover;
    public static bool isPlayerTurn { get; private set; }

    void Awake () {
        PlayerBattleController.menuItem = actionMenuItem;
        PlayerBattleController.menuItemHovered = actionMenuItemHover;
        ai = this.gameObject.GetComponent<AIBattleController>();
        player = this.gameObject.GetComponent<PlayerBattleController>();
        ////// DEBUG CODE //////
        if (ai == null)
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

    public static void EndCurrentTurn() {
        if (isPlayerTurn) {
            player.EndTurn();
            isPlayerTurn = false;
            ai.StartTurn();
            Debug.Log("BattleController - Starting AI Turn");
        } else {
            ai.EndTurn();
            player.StartTurn();
            isPlayerTurn = true;
            Debug.Log("BattleController - Starting Player Turn");
        }
    }

}

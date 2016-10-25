using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// This class will be responsible for handling Game Loop States

public class BattleController : MonoBehaviour {
    public AIBattleController ai;
    public PlayerBattleController player;
    public VictoryCondition victoryCondition;
    private bool nextSceneLoaded;

    // set in editor
    public Texture2D actionMenuItem;
    public Texture2D actionMenuItemHover;
    
    public EndBattleBanner endBanner;

    public static bool IsPlayerTurn { get; private set; }
    public bool BattleIsDone { get; private set; }

    void Awake () {
        PlayerBattleController.menuItem = actionMenuItem;
        PlayerBattleController.menuItemHovered = actionMenuItemHover;
        ai = this.gameObject.GetComponent<AIBattleController>();
        player = this.gameObject.GetComponent<PlayerBattleController>();
        victoryCondition = this.gameObject.GetComponent<VictoryCondition>();
        ////// DEBUG CODE //////
        if (ai == null) {
            Debug.Log("Error :: AI Battle Controller not defined -> BattleController.cs");
        }
        if (player == null) {
            Debug.Log("Error :: Player Battle Controller not defined -> BattleController.cs");
        }
        if(victoryCondition == null) {
            Debug.Log("Error :: VictoryCondition not defined -> BattleController.cs");
        }
        ////////////////////////
    }

    void Start() {
        IsPlayerTurn = true;
        BattleIsDone = false;
        nextSceneLoaded = false;
    }

    void Update() {
        if(!BattleIsDone){
            if (victoryCondition.Achieved()) {
                endBanner.ShowWin();
                EndBattle();
            } else if (player.IsAnnihilated()) {
                endBanner.ShowLoss();
                EndBattle();
            }
        } else if (!nextSceneLoaded && Input.GetMouseButtonDown(0)) {
            if (GameManager.instance) {
                GameManager.instance.UpdateArmyAfterBattle();
            }
            nextSceneLoaded = true; // prevent skipping scenes by spam click
            LoadNextScene();
        }
    }

    public bool CanEndTurn() {
        return IsPlayerTurn ? (player.AllUnitsDone() && ai.NoneAttacking() && player.NoneAttacking()) : (ai.AllUnitsDone() && player.NoneAttacking() && ai.NoneAttacking());
    }

    public void EndCurrentTurn() {
        if (IsPlayerTurn) {
            player.EndTurn();
            IsPlayerTurn = false;
            ai.StartTurn();
            Debug.Log("BattleController - Starting AI Turn");
        } else {
            ai.EndTurn();
            player.StartTurn();
            IsPlayerTurn = true;
            Debug.Log("BattleController - Starting Player Turn");
        }
    }
    
    private void EndBattle() {
        player.EndTurn();
        ai.EndTurn();
        BattleIsDone = true;
    }

    private void LoadNextScene() {
        LevelManager.RegisterVictory(victoryCondition.Achieved());
        if (victoryCondition.Achieved()) {
            LevelManager.LoadNextScene();
        } else if (player.IsAnnihilated()) {
            LevelManager.ReturnToWorldMap();
        }
    }
}

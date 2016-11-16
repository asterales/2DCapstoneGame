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
    public TurnTransition turnTransition;

    public static bool IsPlayerTurn { get; private set; }
    public bool BattleIsDone { get; private set; }
    public static bool PlayerWon { get; private set; }
    public static int numTurns = 0;

    void Awake () {
        PlayerBattleController.menuItem = actionMenuItem;
        PlayerBattleController.menuItemHovered = actionMenuItemHover;
        ai = GetComponent<AIBattleController>();
        player = GetComponent<PlayerBattleController>();
        victoryCondition = GetComponent<VictoryCondition>();
        numTurns = 0;
        InitFlags();
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

    private void InitFlags() {
        IsPlayerTurn = true;
        BattleIsDone = false;
        nextSceneLoaded = false;
        PlayerWon = false;
    }

    void Update() {
        if(!BattleIsDone){
            if (victoryCondition.Achieved()) {
                PlayerWon = true;
                endBanner.ShowWin();
                EndBattle();
            } else if (player.IsAnnihilated()) {
                endBanner.ShowLoss();
                EndBattle();
            }
        } else if (endBanner.FinishedDisplay && !nextSceneLoaded && Input.GetMouseButtonDown(0)) {
            UpdateArmyAfterBattle();
            nextSceneLoaded = true; // prevent skipping scenes by spam click
            LoadNextScene();
        }
    }

    private void UpdateArmyAfterBattle() {
        if (GameManager.instance) {
            CustomUnitLoader unitLoader = BattleControllerManager.instance.unitLoader;
            if (PlayerWon && unitLoader && unitLoader.CanReplaceUnits()) {
                unitLoader.ReplacePlayerArmy();
            } else {
                if (player.IsAnnihilated()) {
                    player.RestoreInitialArmyState();
                }
                GameManager.instance.UpdateArmyAfterBattle();
            }
        }
    }

    public bool CanEndTurn() {
        if (!BattleIsDone) {
            if (IsPlayerTurn) {
                return player.AllUnitsDone() && ai.NoneAttacking() && player.NoneAttacking();
            }
            return ai.AllUnitsDone() && player.NoneAttacking() && ai.NoneAttacking();
        }
        return false;
    }

    public void EndCurrentTurn() {
        if (!turnTransition.IsRunning) {
            turnTransition.PlayTransition(IsPlayerTurn, SwitchTurns);
        }
    }

    private void SwitchTurns() {
        HexMap.ClearAttackTiles();
        if (IsPlayerTurn) {
            player.EndTurn();
            IsPlayerTurn = false;
            ai.StartTurn();
        } else {
            ai.EndTurn();
            player.StartTurn();
            IsPlayerTurn = true;
            numTurns++;
        }
    }
    
    private void EndBattle() {
        player.EndTurn();
        ai.EndTurn();
        BattleIsDone = true;
        SelectionController.ClearAllSelections();
    }

    private void LoadNextScene() {
        LevelManager lm = LevelManager.activeInstance;
        if (lm) {
            lm.RegisterVictory(victoryCondition.Achieved());
            if (PlayerWon) {
                lm.NextScene();
            } else if (GameManager.instance && !GameManager.instance.HasPassedFirstLevel()){
                // failed the tutorial level...go back to main menu
                LevelManager.ReturnToMainMenu();
            } else {
                LevelManager.ReturnToWorldMap();
            }
        }
    }
}

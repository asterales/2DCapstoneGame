using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// This class will be responsible for handling Game Loop States

public class BattleController : MonoBehaviour {
    public AIBattleController ai;
    public PlayerBattleController player;
    public ScriptedAIBattleController scriptedAI;
    public VictoryCondition victoryCondition;
    private bool nextSceneLoaded;

    // set in editor
    public Texture2D actionMenuItem;
    public Texture2D actionMenuItemHover;
    
    // end of battle banners
    public Image winBanner;
    public Image lossBanner;
    private Vector3 bannerVisiblePos; //hack

    // turn banners
    public Image playerTurnBanner;
    public Image enemyTurnBnner;
    public bool showTurnBanner; // to work with game dialogue manager

    private bool isPlayerTurn;

    public bool BattleIsDone { get; private set; }

    void Awake () {
        showTurnBanner = true;
        PlayerBattleController.menuItem = actionMenuItem;
        PlayerBattleController.menuItemHovered = actionMenuItemHover;
        ai = this.gameObject.GetComponent<AIBattleController>();
        player = this.gameObject.GetComponent<PlayerBattleController>();
        scriptedAI = this.gameObject.GetComponent<ScriptedAIBattleController>();
        victoryCondition = this.gameObject.GetComponent<VictoryCondition>();
        ////// DEBUG CODE //////
        if (ai == null && scriptedAI == null) {
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

    void Update() {
        DisplayTurnBanner();
        if(!BattleIsDone){
            if (victoryCondition.Achieved()) {
                DisplayWin();
                EndBattle();
            } else if (player.IsAnnihilated()) {
                DisplayLoss();
                EndBattle();
            }
        } else if (!nextSceneLoaded && Input.GetMouseButtonDown(0)) {
            nextSceneLoaded = true; // prevent skipping scenes by spam click
            LoadNextScene();
        }
    }

    void Start() {
        bannerVisiblePos = winBanner.gameObject.transform.position;
        winBanner.gameObject.transform.position = new Vector3(-1000, -1000, 0);
        lossBanner.gameObject.transform.position = new Vector3(-1000, -1000, 0);
        isPlayerTurn = true;
        BattleIsDone = false;
        nextSceneLoaded = false;
    }

    private void DisplayTurnBanner() {
       if (showTurnBanner) {
           if (isPlayerTurn){
               playerTurnBanner.enabled = true;
               enemyTurnBnner.enabled = false;
           } else {
               playerTurnBanner.enabled = false;
               enemyTurnBnner.enabled = true;
           }
       } else {
           playerTurnBanner.enabled = false;
           enemyTurnBnner.enabled = false;
       }
    }

    public void EndCurrentTurn() {
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

    private void DisplayWin() {
        Debug.Log("Player Wins!");
        winBanner.gameObject.transform.position = bannerVisiblePos;
    }

    private void DisplayLoss() {
        Debug.Log("Player lost!");
        lossBanner.gameObject.transform.position = bannerVisiblePos;
    }

    private void EndBattle() {
        player.EndTurn();
        ai.EndTurn();
        BattleIsDone = true;
    }

    private void LoadNextScene() {
        if (victoryCondition.Achieved()) {
            LevelManager.LoadNextScene();
        } else if (player.IsAnnihilated()) {
            Debug.Log("Return to World Map");
        }
    }
}

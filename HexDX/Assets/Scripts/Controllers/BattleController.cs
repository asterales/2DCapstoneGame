using UnityEngine;
using System.Collections.Generic;

// This class will be responsible for handling Game Loop States

public class BattleController : MonoBehaviour {
    private AIBattleController ai;
    private PlayerBattleController player;
    private bool playerTurn;
    public Unit currentUnit;

    void Awake () {
        ai = this.gameObject.GetComponent<AIBattleController>();
        player = this.gameObject.GetComponent<PlayerBattleController>();
        currentUnit = null;
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
        //Starting player selection
        playerTurn = (int)Mathf.Floor(Random.Range(0, 2)) == 1;
        SetControllerForTurn();
    }

    private void SetControllerForTurn(){
        if(playerTurn){
            Debug.Log("Player Turn enabled");
            ai.enabled = false;
            player.enabled = true;
            //player.FinishedTurn = false; //clear finished flag
        }else {
            Debug.Log("AI Turn enabled");
            player.enabled = false;
            ai.enabled = true;
            //ai.FinishedTurn = false; //clear finished flag
        }
    }

    private bool TurnCompleted() {
        //return playerTurn ? player.FinishedTurn : ai.FinishedTurn;
        return false;
    }

    // Update is called once per frame
    void Update () {
        if (TurnCompleted()){
            playerTurn = !playerTurn;
            SetControllerForTurn();
        }
    }
}

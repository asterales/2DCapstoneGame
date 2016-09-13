using UnityEngine;
using System.Collections.Generic;

// This class will be responsible for handling Game Loop States

public class BattleController : MonoBehaviour {
    public AIBattleController ai;
    public PlayerBattleController player;
    private bool playerTurn;

    void Awake () {
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
        InitUnitLists();
        playerTurn = true;
    }

    private void InitUnitLists() {
        Unit[] allUnits = FindObjectsOfType(typeof(Unit)) as Unit[];
        foreach (Unit unit in allUnits) {
            if (unit.isPlayerUnit) {
                player.units.Add(unit);
            } else {
                ai.units.Add(unit);
            }
        }
    }

    public void EndCurrentTurn() {
        if (playerTurn) {
            player.EndTurn();
            playerTurn = false;
            ai.StartTurn();
            Debug.Log("BattleController - Starting AI Turn");
        } else {
            ai.EndTurn();
            player.StartTurn();
            playerTurn = true;
            Debug.Log("BattleController - Starting Player Turn");
        }
    }
}

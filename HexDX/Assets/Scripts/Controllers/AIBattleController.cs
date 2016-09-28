using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AIBattleController : MonoBehaviour {
    private BattleController battleController;
    public List<UnitAI> unitAIs;

    //keeping track of last unit being modified last update
    private int currentUnitIndex;

    void Start() {
        InitUnitLists();
        battleController = gameObject.GetComponent<BattleController>();
    }

    private void InitUnitLists() {
        Unit[] allUnits = FindObjectsOfType(typeof(Unit)) as Unit[];
        List<Unit> playerUnits = new List<Unit>();
        unitAIs = new List<UnitAI>();
        foreach(Unit unit in allUnits) {
            UnitAI ai = unit.gameObject.GetComponent<UnitAI>();
            if (ai){
                unitAIs.Add(ai);
            } else {
                playerUnits.Add(unit);
            }
        }
        UnitAI.SetPlayerUnits(playerUnits);

        //DEBUGGING CODE//
        for(int i = 0; i < unitAIs.Count; i++) {
            unitAIs[i].unitNum = i;
        }
    }

    void Update() {
        if (SelectionController.TakingAIInput()){
            if (currentUnitIndex < unitAIs.Count) {
                UnitAI ai = unitAIs[currentUnitIndex];
                if (ai) {
                    //Debug.Log(currentUnitIndex + " " + ai.unit.phase);
                    switch(ai.unit.phase) {
                        case UnitTurn.Open:
                            ai.SetMovement();
                            break;
                        case UnitTurn.Facing:
                            ai.SetFacing();
                            break;
                        case UnitTurn.ChoosingAction:
                            ai.SetAction();
                            break;
                        case UnitTurn.Attacking:
                            ai.SetAttack();
                            break;
                        case UnitTurn.Done:
                            ai.Reset();
                            currentUnitIndex++;
                            break;
                        default:
                            break;
                    }
                } else {
                    currentUnitIndex++;
                } 
            } else {
                battleController.EndCurrentTurn();
            }
        }
    }

    private void ResetUnit() {
        Debug.Log ("Resetting "+ currentUnitIndex);
        SelectionController.HideTarget();
    }

    public void StartTurn() {
        Debug.Log("number of units" + unitAIs.Count);
        currentUnitIndex = 0;
        SelectionController.mode = SelectionMode.AITurn;
    }

    public void EndTurn() {
        for (int i = 0; i < unitAIs.Count; i++) {
            if(unitAIs[i]) {
                unitAIs[i].unit.MakeOpen();
            }
        }
    }
}

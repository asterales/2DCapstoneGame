using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AIBattleController : ArmyBattleController {
    //keeping track of last unit being modified last update
    private int currentUnitIndex;

    public override void InitUnitList() {
        Unit[] allUnits = BattleControllerManager.instance.hexMap.GetUnitsOnMap();
        units = new List<Unit>();
        List<Unit> playerUnits = new List<Unit>();

        for(int i = 0; i < allUnits.Length; i++) {
            UnitAI ai = allUnits[i].GetComponent<UnitAI>();
            if (ai) {
                units.Add(allUnits[i]);
                ai.unitNum = i; // for debugging
            } else {
                playerUnits.Add(allUnits[i]);
            }
        }
        UnitAI.SetPlayerUnits(playerUnits);
    }

    void Update() {
        if (!battleController.BattleIsDone && SelectionController.TakingAIInput()){
            if (currentUnitIndex < units.Count) {
                if (units[currentUnitIndex].enabled) {
                    UnitAI ai = GetAI(currentUnitIndex);
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
            } else if (battleController.CanEndTurn()){
                battleController.EndCurrentTurn();
            }
        }
    }

    public Unit GetUnit() {
        return currentUnitIndex < units.Count ? units[currentUnitIndex] :  null;
    }

    private void ResetUnit() {
        //Debug.Log ("Resetting "+ currentUnitIndex);
        SelectionController.HideTarget();
    }

    public override void StartTurn() {
        //Debug.Log("number of units" + units.Count);
        Debug.Log("Starting AI Turn");
        currentUnitIndex = 0;
        SelectionController.mode = SelectionMode.AITurn;
        base.StartTurn();
    }

    private UnitAI GetAI(int index) {
        if (units[index] == null) Debug.Log("ERROR :: UNIT IS NULL");
        if (units[index].GetComponent<UnitAI>() == null) Debug.Log("ERROR :: AI COMPONENT IS NULL");
        return units[index] != null ? units[index].GetComponent<UnitAI>() : null;
    }
}

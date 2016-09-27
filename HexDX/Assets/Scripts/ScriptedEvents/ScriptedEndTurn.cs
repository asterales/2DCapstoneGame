using UnityEngine;
using System.Collections;

public class ScriptedEndTurn : ScriptEvent {
    public PlayerBattleController player;
    public ScriptedAIBattleController scriptedAI;

    public override void DoPlayerEvent() {
        SelectionController.mode = SelectionMode.ScriptedPlayerEndTurn;
    }

    public override void DoEvent() {
        FinishEvent();
    }

    public override void FinishEvent(){
        if(isPlayerEvent){
            player.EndTurn();
            scriptedAI.StartTurn();
        } else {
            scriptedAI.EndTurn();
            player.StartTurn();
        }

    	base.FinishEvent();
    }
}

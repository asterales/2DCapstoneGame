using UnityEngine;
using System.Collections;

public class ScriptedEndTurn : ScriptEvent {
    public PlayerBattleController player;
    public ScriptedAIBattleController scriptedAI;

    public override void DoPlayerEvent() {
        SelectionController.mode = SelectionMode.ScriptedPlayerEndTurn;
    }

    public override void DoEvent() {
        SelectionController.mode = SelectionMode.ScriptedAI;
        Debug.Log("AI is FinishingTurn");
    }

    public override void FinishEvent(){
        if(isPlayerEvent){
            player.EndTurn();
        } else {
            scriptedAI.EndTurn();
        }

    	base.FinishEvent();
    }
}

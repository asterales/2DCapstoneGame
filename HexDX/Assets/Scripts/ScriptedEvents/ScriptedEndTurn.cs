using UnityEngine;
using System.Collections;

public class ScriptedEndTurn : ScriptEvent {
	public PlayerBattleController player;
    public override void DoPlayerEvent() {
        SelectionController.mode = SelectionMode.ScriptedPlayerEndTurn;
        player.enabled = true; // hack
    }

    public override void DoEvent() {
        SelectionController.mode = SelectionMode.ScriptedAI;
        Debug.Log("AI is FinishingTurn");
    }

    public override void FinishEvent(){
    	player.enabled = false;
    	base.FinishEvent();
    }
}

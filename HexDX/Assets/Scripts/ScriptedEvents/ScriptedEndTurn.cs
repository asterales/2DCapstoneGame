using UnityEngine;

public class ScriptedEndTurn : ScriptEvent {
    
    public override void DoPlayerEvent() {
        SelectionController.mode = SelectionMode.ScriptedPlayerEndTurn;
        Camera.main.transform.GetChild(0).FindChild("EndTurnButton").GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    public override void DoEvent() {
        FinishEvent();
    }

    public override void FinishEvent(){
        PlayerBattleController player = BattleControllerManager.instance.player;
        ScriptedAIBattleController scriptedAI = BattleControllerManager.instance.scriptedAI;
        if(isPlayerEvent){
            Camera.main.transform.GetChild(0).Find("EndTurnButton").GetComponent<SpriteRenderer>().color = Color.white;
            player.EndTurn();
            scriptedAI.StartTurn();
        } else {
            scriptedAI.EndTurn();
            player.StartTurn();
        }

    	base.FinishEvent();
    }
}

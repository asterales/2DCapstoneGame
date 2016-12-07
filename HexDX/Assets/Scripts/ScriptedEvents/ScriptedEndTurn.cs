using UnityEngine;

public class ScriptedEndTurn : ScriptEvent {
    
    public override void DoPlayerEvent() {
        list.sc.mode = SelectionMode.ScriptedPlayerEndTurn;
        Camera.main.transform.Find("AspectRatioController").Find("EndTurnButton").GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    public override void DoEvent() {
        FinishEvent();
    }

    protected override void EarlyCleanUp() {
        Camera.main.transform.GetChild(0).Find("EndTurnButton").GetComponent<SpriteRenderer>().color = Color.white;
    }

    public override void FinishEvent(){
        if (!list.EndEarly) {
            PlayerBattleController player = BattleManager.instance.player;
            ScriptedAIBattleController scriptedAI = BattleManager.instance.scriptedAI;
            if(isPlayerEvent){
                Camera.main.transform.Find("AspectRatioController").Find("EndTurnButton").GetComponent<SpriteRenderer>().color = Color.white;
                player.EndTurn();
                scriptedAI.StartTurn();
            } else {
                scriptedAI.EndTurn();
                player.StartTurn();
            }
        }
    	base.FinishEvent();
    }
}

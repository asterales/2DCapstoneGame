using UnityEngine;
using System.Collections;

public class ScriptedEndTurn : ScriptEvent {
    public PlayerBattleController player;
    public ScriptedAIBattleController scriptedAI;

    protected override void Start() {
        base.Start();
        player = BattleControllerManager.instance.player;
        scriptedAI = BattleControllerManager.instance.scriptedAI;
    }

    public override void DoPlayerEvent() {
        SelectionController.mode = SelectionMode.ScriptedPlayerEndTurn;
        Camera.main.transform.GetChild(0).FindChild("EndTurnButton").gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    public override void DoEvent() {
        FinishEvent();
    }

    public override void FinishEvent(){
        if(isPlayerEvent){
            Camera.main.transform.GetChild(0).Find("EndTurnButton").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            player.EndTurn();
            scriptedAI.StartTurn();
        } else {
            scriptedAI.EndTurn();
            player.StartTurn();
        }

    	base.FinishEvent();
    }
}

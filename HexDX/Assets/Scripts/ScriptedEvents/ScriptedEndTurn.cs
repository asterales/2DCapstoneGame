using UnityEngine;
using System.Collections;

public class ScriptedEndTurn : ScriptEvent {
    public override void DoPlayerEvent() {
        SelectionController.mode = SelectionMode.ScriptedPlayerEndTurn;
        // to be implemented
    }

    public override void DoEvent() {
        SelectionController.mode = SelectionMode.ScriptedAI;
        Debug.Log("AI is FinishingTurn");
        // to be implemented
    }
}

using UnityEngine;
using System.Collections;

public class ScriptedEndTurn : ScriptEvent
{
    public override void StartEvent()
    {
        if (!playerEvent)
        {
            DoEvent();
            return;
        }
        SelectionController.mode = SelectionMode.ScriptedPlayerEndTurn;
        // to be implemented
    }

    public override void DoEvent()
    {
        SelectionController.mode = SelectionMode.ScriptedAI;
        Debug.Log("AI is FinishingTurn");
        // to be implemented
    }

    public override void FinishEvent()
    {
        // to be implemented
        Complete();
    }
}

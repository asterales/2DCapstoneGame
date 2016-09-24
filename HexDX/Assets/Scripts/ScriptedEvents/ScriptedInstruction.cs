using UnityEngine;
using System.Collections;
using System;

public class ScriptedInstruction : ScriptEvent {
    public string instruction;

    public override void StartEvent()
    {
        if (!playerEvent)
        {
            DoEvent();
        }
        // to be implemented
    }

    public override void DoEvent()
    {
        // to be implemented
    }
}

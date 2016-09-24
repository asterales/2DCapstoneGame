using UnityEngine;
using System.Collections.Generic;

public class ScriptList : MonoBehaviour {
    public PlayerBattleController player;
    public AIBattleController ai;
    public List<ScriptEvent> scriptedEvents; // to be done in order and set in UI
    private int currentEvent;

    public void Start()
    {
        currentEvent = 0;
        StartEvents();
    }
	
    public void StartEvents()
    {
        if (scriptedEvents.Count > 0)
        {
            scriptedEvents[0].StartEvent();
        }
    }

	public void NextEvent()
    {
        scriptedEvents[currentEvent].FinishEvent();
        currentEvent++;
        if (scriptedEvents.Count > currentEvent)
        {
            scriptedEvents[currentEvent].StartEvent();
        }
        else
        {
            CompletedScripts();
        }
    }

    private void CompletedScripts()
    {
        // to be implemented
    }
}

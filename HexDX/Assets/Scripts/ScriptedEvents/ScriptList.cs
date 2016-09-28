using UnityEngine;
using System.Collections.Generic;

public class ScriptList : MonoBehaviour {
    public GameDialogueManager dialogueMgr;
    public List<ScriptEvent> scriptedEvents; // to be done in order and set in UI
    private int currentEvent;
    
    public ScriptEvent currentScriptEvent { get { return scriptedEvents[currentEvent]; }}
    public bool EventsCompleted { get; private set; } // flag for completion

    public void Start() {
        EventsCompleted = false;
        currentEvent = 0;
        StartEvents();
    }

    public void StartEvents() {
        if (scriptedEvents.Count > 0) {
            StartInstuctions(scriptedEvents[0]);
        }
    }

	public void NextEvent() {
        currentEvent++;
        if (scriptedEvents.Count > currentEvent) {
            Debug.Log("NEXT " + scriptedEvents[currentEvent].GetType());
            StartInstuctions(scriptedEvents[currentEvent]);
        } else {
            CompletedScripts();
        }
    }

    private void CompletedScripts() {
        Debug.Log("All Scripts Complete");
        EventsCompleted = true;
    }

    private void StartInstuctions(ScriptEvent scriptEvent){
        if(scriptEvent.HasInstructions()){
            SelectionController.mode = SelectionMode.ScriptedPlayerInstruction;
            dialogueMgr.ShowGUI();
            dialogueMgr.SetNewLines(scriptEvent.instructions, scriptedEvents[currentEvent].StartEvent);
        } else {
            scriptedEvents[currentEvent].StartEvent();
        } 
    }
}

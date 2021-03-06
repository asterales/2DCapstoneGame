using UnityEngine;

// For dynamically coded events

public class ScriptedCodeEvent : ScriptedDialogue {
	public delegate void CustomEvent();
	public CustomEvent codeEvent;

    public override void DoPlayerEvent() { 
    	DoCodeEvent();
    }

    public override void DoEvent() { 
    	DoCodeEvent();
    }

    protected override void EarlyCleanUp() { }

    private void DoCodeEvent() {
    	if (codeEvent != null) {
    		codeEvent();
    	}
    }
}
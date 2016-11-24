using UnityEngine;
using System.Collections.Generic;

public abstract class ScriptEvent : MonoBehaviour {
    public bool isPlayerEvent;
    protected ScriptList list;
    public List<string> instructions;

	void Awake () {
        list = GetComponentInParent<ScriptList>();
        ////// DEBUG CODE //////
        if (list == null) {
            Debug.Log("List Component Needs to be defined -> ScriptEvent.cs");
        }
        ////////////////////////
	}

    protected virtual void Start() {
        enabled = false;
    }

    protected void Complete() {
        list.dialogueMgr.hidePromptOnFinish = false;
        list.NextEvent();
    }

    public virtual void StartEvent() {
        if (!list.EndEarly) {
            list.dialogueMgr.hidePromptOnFinish = true;
            enabled = true;
            if (isPlayerEvent) {
                DoPlayerEvent();
            } else {
                DoEvent();
            }
        }
    }

    public virtual void FinishEvent() {
        if (list.EndEarly) {
            EarlyCleanUp();
        }
        enabled = false;
        Complete();
    }
    
    public abstract void DoEvent();
    public abstract void DoPlayerEvent();
    protected abstract void EarlyCleanUp();

    public bool HasInstructions() {
        return instructions != null && instructions.Count > 0;
    }
}

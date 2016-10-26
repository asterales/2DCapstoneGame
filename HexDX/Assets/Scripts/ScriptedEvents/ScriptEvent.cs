using UnityEngine;
using System.Collections.Generic;

public abstract class ScriptEvent : MonoBehaviour {
    protected bool isActive;
    public bool isPlayerEvent;
    protected ScriptList list;
    public List<string> instructions;

	void Awake () {
        list = GetComponentInParent<ScriptList>();
        ////// DEBUG CODE //////
        if (list == null)
        {
            Debug.Log("List Component Needs to be defined -> ScriptEvent.cs");
        }
        ////////////////////////
	}

    protected virtual void Start() {
        isActive = false;
    }

    protected void Complete() {
        list.NextEvent();
    }

    public virtual void StartEvent() {
        isActive = true;
        if (isPlayerEvent) {
            DoPlayerEvent();
        } else {
            DoEvent();
        }
    }

    public virtual void FinishEvent() {
        isActive = false;
        Complete();
    }
    
    public abstract void DoEvent();
    public abstract void DoPlayerEvent();

    public bool HasInstructions() {
        return instructions != null && instructions.Count > 0;
    }
}

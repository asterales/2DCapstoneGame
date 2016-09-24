using UnityEngine;
using System.Collections;

public abstract class ScriptEvent : MonoBehaviour {
    protected bool isActive;
    protected ScriptList list;
    public string instructions;
    public bool playerEvent;

	void Start () {
        list = this.gameObject.GetComponent<ScriptList>();
        ////// DEBUG CODE //////
        if (list == null)
        {
            Debug.Log("List Component Needs to be defined -> ScriptEvent.cs");
        }
        ////////////////////////
	}

    protected void Complete()
    {
        list.NextEvent();
    }

    public abstract void StartEvent();
    public abstract void FinishEvent();
    public abstract void DoEvent();
}

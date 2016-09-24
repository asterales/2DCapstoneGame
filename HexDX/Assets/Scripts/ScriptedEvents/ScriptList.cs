using UnityEngine;
using System.Collections.Generic;

public class ScriptList : MonoBehaviour {
    public List<ScriptEvent> scriptedEvents; // to be done in order and set in UI
    private int currentEvent;

    public void Start()
    {
        currentEvent = 0;
        StartEvents();
    }
	
    public void StartEvents()
    {
        // to be implemented
    }

	public void NextEvent()
    {
        // to be implemented
    }
}

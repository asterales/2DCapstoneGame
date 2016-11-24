using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ScriptList : MonoBehaviour {
    public static readonly Color highlightColor = new Color(1f, 0.84f, 0.87f);

    public GameDialogueManager dialogueMgr;
    public List<ScriptEvent> scriptedEvents; // to be done in order and set in UI
    public SelectionController sc;
    public TutorialController tutorial;
    private int currentEvent;

    public bool EndEarly { get; private set; }
    public ScriptEvent currentScriptEvent { get { return currentEvent < scriptedEvents.Count ? scriptedEvents[currentEvent] : null; }}
    public bool EventsCompleted { get; private set; } // flag for completion

    void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventsCompleted = false;
        tutorial = GetComponent<TutorialController>();
        EndEarly = false;
        enabled = false;
    }

    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (!dialogueMgr) {
            dialogueMgr = FindObjectOfType(typeof(GameDialogueManager)) as GameDialogueManager;
        }
    }

    public void StartEvents() {
        if (scriptedEvents.Count > 0) {
            enabled = true;
            sc = SelectionController.instance;
            EventsCompleted = false;
            currentEvent = 0;
            StartInstuctions(scriptedEvents[0]);
        }
    }

	public void NextEvent() {
        currentEvent++;
        if (scriptedEvents.Count > currentEvent && !EndEarly) {
            if (scriptedEvents[currentEvent] == null) {
                NextEvent();
                return;
            }
            Debug.Log("NEXT " + scriptedEvents[currentEvent].GetType());
            StartInstuctions(scriptedEvents[currentEvent]);
        } else {
            CompletedScripts();
        }
    }

    private void CompletedScripts() {
        Debug.Log("All Scripts Complete");
        EventsCompleted = true;
        enabled = false;
    }

    private void StartInstuctions(ScriptEvent scriptEvent){
        if(scriptEvent.HasInstructions()){
            sc.mode = SelectionMode.ScriptedPlayerInstruction;
            dialogueMgr.ShowGUI();
            dialogueMgr.SetNewLines(scriptEvent.instructions, scriptedEvents[currentEvent].StartEvent);
        } else {
            scriptedEvents[currentEvent].StartEvent();
        } 
    }

    public bool IsFinished() {
        return EventsCompleted || EndEarly;
    }

    void Update() {
        if (Input.GetKeyDown(KeyBindings.SKIP_TUTORIAL)) {
            EndEarly = true;
            if (sc.mode != SelectionMode.ScriptedPlayerInstruction) {
                currentScriptEvent.FinishEvent();
            }
        }
    }
}

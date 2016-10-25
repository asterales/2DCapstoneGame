using UnityEngine;


public class WorldMapTutorial : MonoBehaviour {
	private TutorialInfo info;
	private ScriptList eventsList;

	void Awake() {
		info = GetComponent<TutorialInfo>();
		eventsList = GetComponent<ScriptList>();
	}

	void Start() {
		if (!info.HasBeenCompleted()) {
			eventsList.dialogueMgr.SetSpeaker(info.tutorialAdvisor, info.advisorPortraitIndex);
			eventsList.StartEvents();
		} else {
			enabled = false;
		}
	}

	void Update() {
		if (eventsList.EventsCompleted){
			if(eventsList.dialogueMgr.HasFinishedAllLines()) {
				eventsList.dialogueMgr.HideGUI();
				info.RegisterCompleted();
			}
		}
	}
}
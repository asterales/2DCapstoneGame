using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WorldMapTutorial : MonoBehaviour {
	public List<GameObject> objectsToDisableButtons;
	private List<Button> disabledButtons;

	private TutorialInfo info;
	private ScriptList eventsList;
	private WorldMap worldMap;

	void Awake() {
		worldMap = FindObjectOfType(typeof(WorldMap)) as WorldMap;
		info = GetComponent<TutorialInfo>();
		eventsList = GetComponent<ScriptList>();
	}

	void Start() {
		if (!info.HasBeenCompleted()) {
			worldMap.territories.ForEach(t => t.clickDisabled = true);
			DisableButtons();
			eventsList.dialogueMgr.SetSpeaker(info.tutorialAdvisor, info.advisorPortraitIndex);
			eventsList.StartEvents();
		} else {
			enabled = false;
		}
	}

	private void DisableButtons() {
		disabledButtons = new List<Button>();
		foreach(GameObject obj in objectsToDisableButtons) {
			disabledButtons.AddRange(obj.GetComponentsInChildren<Button>());
		}
		disabledButtons.ForEach(b => b.enabled = false);
	}

	void Update() {
		if (eventsList.EventsCompleted){
			if(eventsList.dialogueMgr.HasFinishedAllLines()) {
				eventsList.dialogueMgr.HideGUI();
				disabledButtons.ForEach(b => b.enabled = true);
				worldMap.territories.ForEach(t => t.clickDisabled = false);
				info.RegisterCompleted();
			}
		}
	}
}
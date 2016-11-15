using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WorldMapTutorial : MonoBehaviour {
	public List<GameObject> objectsToDisableButtons;
	public int minStartingUnits;
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
			SetPermaDeathExplanation();
			eventsList.StartEvents();
		} else {
			gameObject.SetActive(false);
		}
	}

	private void DisableButtons() {
		disabledButtons = new List<Button>();
		foreach(GameObject obj in objectsToDisableButtons) {
			disabledButtons.AddRange(obj.GetComponentsInChildren<Button>());
		}
		disabledButtons.ForEach(b => b.enabled = false);
	}

	private void SetPermaDeathExplanation() {
		ScriptedCodeEvent permaDeathIntro = GetComponentInChildren<ScriptedCodeEvent>();
		GameManager gm = GameManager.instance;
		if (gm.playerAllUnits.Count < minStartingUnits) {
			permaDeathIntro.instructions = new List<string> {
				"From the looks of it, you've lost a few units.",
				"Here's a <b>NEW</b> <b>RIFLEMAN</b> unit for your losses. Be careful next time because you <b>CAN'T</b> <b>GET</b> <b>LOST</b> <b>UNITS</b> <b>BACK</b>.",
			};
			permaDeathIntro.codeEvent = GivePlayerUnit;
		} else {
			permaDeathIntro.instructions = new List<string> {
				"Good job for getting through that last battle without losing anyone. " +
				"Note that you <b>CAN'T</b> <b>GET</b> <b>LOST</b> <b>UNITS</b> <b>BACK</b>."
			};
		}
		eventsList.scriptedEvents.Insert(1, permaDeathIntro);
	}

	private void GivePlayerUnit() {
		GameManager gm = GameManager.instance;
		GameObject newUnitObj = Instantiate<GameObject>(Resources.Load<GameObject>("Units/Rifleman"));
		Unit newUnit = newUnitObj.GetComponent<Unit>();
		gm.AddNewPlayerUnit(newUnit);
		newUnit.gameObject.SetActive(true);
		gm.activeUnits.Add(newUnit);
		gm.ResetUnit(newUnit);
		ActiveArmyDisplay display = FindObjectOfType(typeof(ActiveArmyDisplay)) as ActiveArmyDisplay;
		display.RefreshDisplay();
	}

	void Update() {
		if (eventsList.EventsCompleted){
			if(eventsList.dialogueMgr.HasFinishedAllLines()) {
				eventsList.dialogueMgr.HideGUI();
				disabledButtons.ForEach(b => b.enabled = true);
				worldMap.territories.ForEach(t => t.clickDisabled = false);
				info.RegisterCompleted();
				gameObject.SetActive(false);
			}
		}
	}
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class UnitSelectionPanel : WorldMapPopupPanel {
	private ActiveArmyDisplay activeUnitsDisplay;
	public ActiveArmyDisplay worldMapActiveArmyDisplay;
	public UnitStatDisplay statDisplay;
	private InactiveArmyDisplay inactiveUnitsDisplay;
	private Button saveButton;

	protected override void Awake() {
		base.Awake();
		InitSaveButton();
		statDisplay = transform.Find("Stats Panel").GetComponent<UnitStatDisplay>();
		activeUnitsDisplay = transform.Find("Active Units").GetComponent<ActiveArmyDisplay>();
		inactiveUnitsDisplay = transform.Find("Inactive Units").GetComponent<InactiveArmyDisplay>();
	}

	void Start() {
		AddMouseHandlerToPanels(activeUnitsDisplay);
		AddMouseHandlerToPanels(inactiveUnitsDisplay);
	}

	void Update() {
		saveButton.interactable = activeUnitsDisplay.unitPanels.Where(p => p.unit != null).ToList().Count > 0;
	}

	private void AddMouseHandlerToPanels(ArmyDisplay armyDisplay) {
		foreach(UnitDisplay panel in armyDisplay.unitPanels) {
			UnitSelectionMouseHandler mouseHandler = panel.gameObject.AddComponent<UnitSelectionMouseHandler>();
			mouseHandler.selectionPanel = this;
		}
	}

	private void InitSaveButton() {
		saveButton = transform.Find("Save Button").GetComponent<Button>();
		saveButton.onClick.AddListener(ConfirmArmySelection);
	}

	private void ConfirmArmySelection() {
		GameManager gm = GameManager.instance;
		gm.activeUnits = activeUnitsDisplay.unitPanels.Where(p => p.unit != null).Select(p => p.unit).ToList();
		RefreshDisplays();
	}

	public override void Show() {
		base.Show();
		RefreshDisplays();
	}

	public override void Hide() {
		InactivateInactiveArmy();
		base.Hide();
	}

	private void RefreshDisplays() {
		worldMapActiveArmyDisplay.RefreshDisplay();
		activeUnitsDisplay.RefreshDisplay();
		inactiveUnitsDisplay.RefreshDisplay();
	}

	private void InactivateInactiveArmy() {
		List<Unit> inactiveUnits = GameManager.instance.GetInactiveUnits();
		inactiveUnits.ForEach(u => u.gameObject.SetActive(false));
	}

	public void SwitchUnitToOtherArmy(UnitDisplay unitPanel) {
		UnitDisplay nextDisplayPanel;
		if(activeUnitsDisplay.unitPanels.Contains(unitPanel)) {
			nextDisplayPanel = inactiveUnitsDisplay.GetFirstEmptySlot();
		} else {
			nextDisplayPanel = activeUnitsDisplay.GetFirstEmptySlot();
		}
		if(nextDisplayPanel != null) {
			nextDisplayPanel.unit = unitPanel.unit;
			unitPanel.unit = null;
		}
	}
}
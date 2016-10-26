using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class UnitSelectionPanel : WorldMapPopupPanel {
	public ActiveArmyDisplay activeUnitsDisplay;
	public InactiveArmyDisplay inactiveUnitsDisplay;
	public ActiveArmyDisplay worldMapActiveArmyDisplay;
	public UnitStatDisplay statDisplay;
	private Button saveButton;

	protected override void Awake() {
		base.Awake();
		InitSaveButton();
		statDisplay = transform.Find("Stats Panel").GetComponent<UnitStatDisplay>();
		activeUnitsDisplay = transform.Find("Active Units").GetComponent<ActiveArmyDisplay>();
		inactiveUnitsDisplay = transform.Find("Inactive Units").GetComponent<InactiveArmyDisplay>();
	}

	void Update() {
		saveButton.interactable = activeUnitsDisplay.HasUnits();
	}

	private void InitSaveButton() {
		saveButton = transform.Find("Save Button").GetComponent<Button>();
		saveButton.onClick.AddListener(ConfirmArmySelection);
	}

	private void ConfirmArmySelection() {
		GameManager gm = GameManager.instance;
		gm.activeUnits = activeUnitsDisplay.GetUnits();
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
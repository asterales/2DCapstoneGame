using UnityEngine.UI;
using System.Collections.Generic;

public class UnitSelectionPanel : WorldMapPopupPanel {
	public ActiveArmyDisplay activeUnitsDisplay;
	public InactiveArmyDisplay inactiveUnitsDisplay;
	public ActiveArmyDisplay worldMapActiveArmyDisplay;
	public UnitStatDisplay statDisplay;
	public int minArmySize;
	
	private Button returnButton;
	private UnitDisplay highlightedDisplay;

	protected override void Awake() {
		base.Awake();
		returnButton = transform.Find("Return Button").GetComponent<Button>();
		statDisplay = transform.Find("Stats Panel").GetComponent<UnitStatDisplay>();
		activeUnitsDisplay = transform.Find("Active Units").GetComponent<ActiveArmyDisplay>();
		inactiveUnitsDisplay = transform.Find("Inactive Units").GetComponent<InactiveArmyDisplay>();
		minArmySize = GameManager.MIN_ARMY_SIZE;
	}

	void Update() {
		returnButton.interactable = activeUnitsDisplay.GetUnits().Count >= minArmySize || GameManager.instance.playerAllUnits.Count == 0;
	}

	public void SaveArmySelection() {
		GameManager gm = GameManager.instance;
		gm.activeUnits = activeUnitsDisplay.GetUnits();
		RefreshDisplays();
	}

	public override void Show() {
		base.Show();
		RefreshDisplays();
		UnitDisplay firstActiveUnit = activeUnitsDisplay.GetFirstOccupiedSlot();
		SelectUnit(firstActiveUnit);
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

	public void SwitchHighlightedUnit(UnitDisplay display) {
		ClearHighlightedUnit();
		highlightedDisplay = display;
		display.HighlightSelected();
	}

	public void SelectUnit(UnitDisplay display) {
		if (display) {
			SwitchHighlightedUnit(display);
			statDisplay.DisplayUnitStats(display.unit);
		}
	}

	private void ClearHighlightedUnit() {
		if (highlightedDisplay) {
			highlightedDisplay.UnhighlightSelected();
		}
	}
}
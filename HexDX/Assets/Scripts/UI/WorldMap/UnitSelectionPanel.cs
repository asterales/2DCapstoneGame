using UnityEngine.UI;
using System.Collections.Generic;

public class UnitSelectionPanel : WorldMapPopupPanel {
	public ActiveArmyDisplay activeUnitsDisplay;
	public InactiveArmyDisplay inactiveUnitsDisplay;
	public ActiveArmyDisplay worldMapActiveArmyDisplay;
	public UnitStatDisplay statDisplay;
	public int minArmySize;
	private Button saveButton;

	private UnitDisplay highlightedDisplay;

	protected override void Awake() {
		base.Awake();
		InitSaveButton();
		statDisplay = transform.Find("Stats Panel").GetComponent<UnitStatDisplay>();
		activeUnitsDisplay = transform.Find("Active Units").GetComponent<ActiveArmyDisplay>();
		inactiveUnitsDisplay = transform.Find("Inactive Units").GetComponent<InactiveArmyDisplay>();
		minArmySize = GameManager.MIN_ARMY_SIZE;
	}

	void Update() {
		saveButton.interactable = activeUnitsDisplay.GetUnits().Count >= minArmySize;
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
		ClearHighlightedUnit();
		statDisplay.ClearDisplay();
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

	public void SwitchHighlightedUnit(UnitDisplay display) {
		ClearHighlightedUnit();
		highlightedDisplay = display;
		display.HighlightSelected();
	}

	private void ClearHighlightedUnit() {
		if (highlightedDisplay) {
			highlightedDisplay.UnhighlightSelected();
		}
	}
}
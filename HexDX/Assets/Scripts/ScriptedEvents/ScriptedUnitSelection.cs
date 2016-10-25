using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScriptedUnitSelection : ScriptEvent {
	// set up to make player move one unit over

	public Button changeArmyPanelButton;
	public Button recruitPanelButton;
	public UnitSelectionPanel selectionPanel;

	private int initialActiveNum;
	private int initialInactiveNum;

    public override void DoPlayerEvent() {
    	GameManager gm = GameManager.instance;
    	initialInactiveNum = gm.GetInactiveUnits().Count;
    	initialActiveNum = gm.activeUnits.Count;
    }

    void Update() {
    	if(selectionPanel.enabled && HasMovedInactiveUnit()){

    	}
    }

    private bool HasMovedInactiveUnit() {
    	return initialInactiveNum - selectionPanel.inactiveUnitsDisplay.GetUnits().Count == 1;
    }

    public override void DoEvent() {
    	FinishEvent();
    }
}	
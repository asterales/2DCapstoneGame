using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScriptedUnitDisplaySelection : ScriptEvent {
	// set up to make player move one unit over from inactive panel
	public Button changeArmyPanelButton;
	public Button recruitPanelButton;
	public UnitSelectionMouseHandler unitDisplaySelector;

    public override void DoPlayerEvent() {
    	unitDisplaySelector.doubleClickCallback = OnSelection;
    }

    private void OnSelection() {
        FinishEvent();
    }

    public override void DoEvent() {
    	FinishEvent();
    }
}	
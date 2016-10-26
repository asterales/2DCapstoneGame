using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ScriptedUnitDisplaySelection : ScriptEvent {
	// set up to make player move one unit over from inactive panel
	public Button changeArmyPanelButton;
	public Button recruitPanelButton;
	public UnitSelectionMouseHandler unitDisplaySelector;

    private List<UnitSelectionMouseHandler> handlers;
    private UnitSelectionPanel panel;
    private bool disabledHandlers;

    protected override void Start() {
        base.Start();
        panel = unitDisplaySelector.selectionPanel;
        handlers = unitDisplaySelector.selectionPanel.GetComponentsInChildren<UnitSelectionMouseHandler>().ToList();
    }

    public override void DoPlayerEvent() {
        DisableOtherMouseHandlers();
    	unitDisplaySelector.doubleClickCallback = OnSelection;
        changeArmyPanelButton.GetComponent<Image>().color = ScriptList.highlightColor;
        recruitPanelButton.interactable = false;
    }

    private void DisableOtherMouseHandlers() {
        foreach(UnitSelectionMouseHandler handler in handlers) {
            if (handler != unitDisplaySelector) {
                handler.clickEnabled = false;
            }
        }
    }

    void Update() {
        UnitSelectionPanel panel = unitDisplaySelector.selectionPanel;
        if(panel.isActive && !disabledHandlers) {
            disabledHandlers = true;
            DisableOtherMouseHandlers();
        }
    }

    private void OnSelection() {
        changeArmyPanelButton.GetComponent<Image>().color = Color.white;
        unitDisplaySelector.doubleClickCallback = null;
        recruitPanelButton.interactable = true;
        handlers.ForEach(h => h.clickEnabled = true);
        FinishEvent();
    }

    public override void DoEvent() {
    	FinishEvent();
    }
}	
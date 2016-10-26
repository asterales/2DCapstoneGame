using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ScriptedArmySelection : ScriptEvent {
	// set up to make player move one unit over from inactive panel
	public Button changeArmyPanelButton;
	public Button recruitPanelButton;
	public Button saveButton;
	public UnitSelectionPanel panel;
	public int minNumActiveUnits;

    public override void DoPlayerEvent() {
    	if (minNumActiveUnits > GameManager.instance.playerAllUnits.Count) {
    		Debug.Log("Target number of units is greater than number of available units! - ScriptedArmySelection.cs");
    	}
    	recruitPanelButton.interactable = false;
    	changeArmyPanelButton.GetComponent<Image>().color = ScriptList.highlightColor;
    	saveButton.onClick.AddListener(OnSave);
    }

    private void OnSave() {
    	saveButton.onClick.RemoveListener(OnSave);
    	saveButton.GetComponent<Image>().color = Color.white;
    	recruitPanelButton.interactable = true;
    	panel.minArmySize = GameManager.MIN_ARMY_SIZE;
    	changeArmyPanelButton.GetComponent<Image>().color = Color.white;
    	FinishEvent();
    }

    void Update() {
    	if (isActive && panel.isActive) {
    		if(panel.minArmySize != minNumActiveUnits){
    			panel.minArmySize = minNumActiveUnits;
    		} else {
    			if(panel.activeUnitsDisplay.GetUnits().Count >= minNumActiveUnits) {
		    		saveButton.GetComponent<Image>().color = ScriptList.highlightColor;
		    	} else {
		    		saveButton.GetComponent<Image>().color = Color.white;
		    	}
    		}
    	}
    }

    public override void DoEvent() {
    	FinishEvent();
    }
}	
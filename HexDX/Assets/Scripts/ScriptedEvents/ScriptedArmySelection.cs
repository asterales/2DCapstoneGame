using UnityEngine;
using UnityEngine.UI;

public class ScriptedArmySelection : ScriptedButtonPress {
	// set up to make player move one unit over from inactive panel
	public UnitSelectionPanel panel;
	public int minNumActiveUnits;

    public override void DoPlayerEvent() {
    	if (minNumActiveUnits > GameManager.instance.playerAllUnits.Count) {
    		Debug.Log("Target number of units is greater than number of available units! - ScriptedArmySelection.cs");
    	}
        base.DoPlayerEvent();
    }

    protected override void OnClick() {
        panel.minArmySize = GameManager.MIN_ARMY_SIZE;
        base.OnClick();
    }

    void Update() {
    	if (isActive && panel.isActive) {
    		if(panel.minArmySize != minNumActiveUnits){
    			panel.minArmySize = minNumActiveUnits;
    		} else {
    			if(panel.activeUnitsDisplay.GetUnits().Count >= minNumActiveUnits) {
		    		mainButton.GetComponent<Image>().color = ScriptList.highlightColor;
		    	} else {
		    		mainButton.GetComponent<Image>().color = Color.white;
		    	}
    		}
    	}
    }

    public override void DoEvent() {
        FinishEvent();
    }
}	
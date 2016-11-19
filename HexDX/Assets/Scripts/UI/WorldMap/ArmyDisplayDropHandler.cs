using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArmyDisplayDropHandler : MonoBehaviour, IDropHandler {
	private UnitSelectionPanel selectionPanel;
	private ArmyDisplay armyDisplay;

	void Awake() {
		selectionPanel = GetComponentInParent<UnitSelectionPanel>();
		armyDisplay = GetComponent<ArmyDisplay>();
	}

	public void OnDrop(PointerEventData eventData) {
		GameObject droppedObj = eventData.pointerDrag;
		if (droppedObj) {
			UnitDisplay droppedDisplay = droppedObj.GetComponent<UnitDisplay>();
			if (!armyDisplay.unitPanels.Contains(droppedDisplay)) {
				SwitchUnitIntoArmy(droppedDisplay);
			}
		}
	}

	private void SwitchUnitIntoArmy(UnitDisplay droppedDisplay) {
		UnitDisplay nextDisplay = armyDisplay.GetFirstEmptySlot();
		if (nextDisplay) {
			nextDisplay.unit = droppedDisplay.unit;
			droppedDisplay.unit = null;
			selectionPanel.SwitchHighlightedUnit(nextDisplay);
		}
	}
}
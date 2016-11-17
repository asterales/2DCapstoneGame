using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSelectionMouseHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler{
	public UnitSelectionPanel selectionPanel;
	public UnitDisplay displayPanel;
	private static UnitDisplay selectedDisplay;

	void Awake() {
		displayPanel = GetComponent<UnitDisplay>();
	}

	public void OnPointerClick(PointerEventData eventData) {
		SwitchHighlightedUnit(displayPanel);
        selectionPanel.statDisplay.DisplayUnitStats(displayPanel.unit);
        if (eventData.clickCount == 2) {
			UnitDisplay nextDisplay = selectionPanel.SwitchUnitToOtherArmy(displayPanel);
			SwitchHighlightedUnit(nextDisplay);
		}
	}

	private void SwitchHighlightedUnit(UnitDisplay display) {
		if (selectedDisplay) {
			selectedDisplay.UnhighlightSelected();
		}
		selectedDisplay = display;
		display.HighlightSelected();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		//selectionPanel.statDisplay.DisplayUnitStats(displayPanel.unit);
	}

	public void OnPointerExit(PointerEventData eventData) {
		//selectionPanel.statDisplay.ClearDisplay();
	}
}
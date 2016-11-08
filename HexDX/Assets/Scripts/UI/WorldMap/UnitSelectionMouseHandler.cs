using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSelectionMouseHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler{
	public UnitSelectionPanel selectionPanel;
	public UnitDisplay displayPanel;

	void Awake() {
		displayPanel = GetComponent<UnitDisplay>();
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.clickCount == 2) {
			selectionPanel.SwitchUnitToOtherArmy(displayPanel);
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		selectionPanel.statDisplay.DisplayUnitStats(displayPanel.unit);
	}

	public void OnPointerExit(PointerEventData eventData) {
		selectionPanel.statDisplay.ClearDisplay();
	}
}
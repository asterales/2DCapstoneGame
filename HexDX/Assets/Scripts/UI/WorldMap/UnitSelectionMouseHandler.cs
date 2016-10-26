using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitSelectionMouseHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler{
	public UnitSelectionPanel selectionPanel;
	private UnitDisplay displayPanel;
	
	public delegate void OnDoubleClick();
	public OnDoubleClick doubleClickCallback;

	void Awake() {
		displayPanel = gameObject.GetComponent<UnitDisplay>();
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.clickCount == 2) {
			selectionPanel.SwitchUnitToOtherArmy(displayPanel);
			if (doubleClickCallback != null) {
				doubleClickCallback();
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		selectionPanel.statDisplay.DisplayUnit(displayPanel.unit);
	}

	public void OnPointerExit(PointerEventData eventData) {
		selectionPanel.statDisplay.ClearDisplay();
	}
}
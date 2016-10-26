using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitSelectionMouseHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler{
	public UnitSelectionPanel selectionPanel;
	public UnitDisplay displayPanel;
	public bool clickEnabled;
	
	public delegate void OnDoubleClick();
	public OnDoubleClick doubleClickCallback;

	void Awake() {
		clickEnabled = true;
		displayPanel = gameObject.GetComponent<UnitDisplay>();
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (clickEnabled && eventData.clickCount == 2) {
			if (doubleClickCallback != null) {
				doubleClickCallback();
			}
			selectionPanel.SwitchUnitToOtherArmy(displayPanel);
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		selectionPanel.statDisplay.DisplayUnit(displayPanel.unit);
	}

	public void OnPointerExit(PointerEventData eventData) {
		selectionPanel.statDisplay.ClearDisplay();
	}
}
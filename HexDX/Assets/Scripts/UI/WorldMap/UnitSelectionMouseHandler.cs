using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitSelectionMouseHandler : MonoBehaviour, IPointerClickHandler,
										 IBeginDragHandler, IDragHandler, IEndDragHandler {
	public UnitSelectionPanel selectionPanel;
	public UnitDisplay displayPanel;

	// Drag/Drop variables
	private static RectTransform draggedRect;
	private static Image draggedImage;
	private static Vector2 formerPosition;

	void Awake() {
		displayPanel = GetComponent<UnitDisplay>();
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (displayPanel.unit) {
			selectionPanel.SwitchHighlightedUnit(displayPanel);
	        selectionPanel.statDisplay.DisplayUnitStats(displayPanel.unit);
		}
	}

	public void OnBeginDrag(PointerEventData eventData) {
		if (displayPanel.unit) {
			OnPointerClick(eventData);
			draggedRect = transform.parent.GetComponent<RectTransform>(); // image is child of UIMask
			draggedImage = GetComponent<Image>();
			draggedImage.raycastTarget = false;
			formerPosition = draggedRect.anchoredPosition;
		}
	}

	public void OnDrag(PointerEventData eventData) {
		if (draggedRect) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			draggedRect.position = new Vector3(mousePos.x, mousePos.y, draggedRect.position.z);
		}
	}

	public void OnEndDrag(PointerEventData eventData) {
		if (draggedRect) {
			draggedRect.anchoredPosition = formerPosition;
			draggedImage.raycastTarget = true;
			draggedRect = null;
		}
		
	}
}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitDisplayClickHandler : MonoBehaviour, IPointerClickHandler {
	private UnitDisplay displayPanel;
	public delegate void OnClick(UnitDisplay unitPanel);
	public OnClick onSingleClickCallback;
	public OnClick onDoubleClickCallback;

	void Awake() {
		displayPanel = gameObject.GetComponent<UnitDisplay>();
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.clickCount == 1 && onSingleClickCallback != null) {
			onSingleClickCallback(displayPanel);
		}else if (eventData.clickCount == 2 && onDoubleClickCallback != null) {
			onDoubleClickCallback(displayPanel);
		}
	}
}
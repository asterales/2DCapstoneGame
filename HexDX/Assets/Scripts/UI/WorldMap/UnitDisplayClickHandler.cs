using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitDisplayClickHandler : MonoBehaviour, IPointerClickHandler {
	private UnitDisplay displayPanel;
	public delegate void OnDoubleClick(UnitDisplay unitPanel);
	public OnDoubleClick onDoubleClickCallback;

	void Awake() {
		displayPanel = gameObject.GetComponent<UnitDisplay>();
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.clickCount == 2) {
			onDoubleClickCallback(displayPanel);
		}
	}
}
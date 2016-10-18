using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	private Image unitPanel;
	public Unit unit;
	private Color spriteColor;

	void Awake() {
		spriteColor = Color.white;
		unitPanel = gameObject.GetComponent<Image>();
	}

	void Update() {
		if (unit != null) {
			unitPanel.enabled = true;
			unitPanel.sprite = unit.spriteRenderer.sprite;
			unitPanel.color = spriteColor;
		} else {
			unitPanel.enabled = false;
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		spriteColor = Color.gray;
	}

	public void OnPointerExit(PointerEventData eventData) {
		spriteColor = Color.white;
	}
}
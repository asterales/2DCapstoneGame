using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	private Image unitPanel;
	public Unit unit;
	private Color spriteColor;

	private static readonly Color hoverColor = Color.white;
	private static readonly Color defaultColor = new Color(0.78f, 0.78f, 0.78f);

	void Awake() {
		spriteColor = defaultColor;
		unitPanel = GetComponent<Image>();
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
		spriteColor = hoverColor;
	}

	public void OnPointerExit(PointerEventData eventData) {
		spriteColor = defaultColor;
	}
}
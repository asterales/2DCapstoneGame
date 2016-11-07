using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	private Image unitPanel;
	public Unit unit;
	private Color spriteColor;
	public UnitDisplayType displayType;
	public bool disableHoverEffect;

	private static readonly Color hoverColor = Color.white;
	private static readonly Color defaultColor = new Color(0.78f, 0.78f, 0.78f);

	void Awake() {
		spriteColor = disableHoverEffect ? hoverColor : defaultColor;
		unitPanel = GetComponent<Image>();
	}

	void Update() {
		if (unit != null) {
			unitPanel.enabled = true;
			unitPanel.sprite = GetSprite();
			unitPanel.color = spriteColor;
		} else {
			unitPanel.enabled = false;
		}
	}

	private Sprite GetSprite() {
		if (displayType == UnitDisplayType.UnitCard) {
			return unit.sprites.unitCard;
		} else {
			return unit.spriteRenderer.sprite;
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		if (!disableHoverEffect) {
			spriteColor = hoverColor;
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		if (!disableHoverEffect) {
			spriteColor = defaultColor;
		}
	}
}

public enum UnitDisplayType {
	AnimatedUnit,
	UnitCard
}
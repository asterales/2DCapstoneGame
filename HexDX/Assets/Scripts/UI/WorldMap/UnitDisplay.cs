using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public Unit unit;
	public UnitDisplayType displayType;
	public bool disableHoverEffect;

	private Color spriteColor;
	private Image unitPanel;
	private bool isHovered;

	private static readonly Color highlightedColor = Color.white;
	private static readonly Color hoverColor = new Color(0.8f, 0.8f, 0.8f);
	private static readonly Color defaultColor = new Color(0.5f, 0.5f, 0.5f);

	void Awake() {
		spriteColor = disableHoverEffect ? highlightedColor : defaultColor;
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
		isHovered = true;
		if (!disableHoverEffect) {
			spriteColor = hoverColor;
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		isHovered = false;
		if (!disableHoverEffect) {
			spriteColor = defaultColor;
		}
	}

	public void HighlightSelected() {
		disableHoverEffect = true;
		spriteColor = highlightedColor;
	}

	public void UnhighlightSelected() {
		disableHoverEffect = false;
		spriteColor = isHovered ? hoverColor : defaultColor;
	}
}

public enum UnitDisplayType {
	AnimatedUnit,
	UnitCard
}
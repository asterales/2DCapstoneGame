using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

// implements IPointer interfaces so will be properly inactive when UI elemnents overlap it
public class Territory : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    private static readonly Color hoverColor = Color.white;
    private static readonly Color capturedColor = new Color(0.24f, 0.71f, 1f);
    private static readonly Color activeColor = new Color(0.8f, 0.8f, 0.8f);
    private static readonly Color inactiveColor = new Color(0.5f, 0.5f, 0.5f);
    
    public List<Territory> neighbors;
    public LevelManager lm;
    public bool captured;
    public bool active;
    public bool clickDisabled;
    private SpriteRenderer spriteRenderer;

    public void Awake() {
        clickDisabled = false;
    }

    public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetDefaultColor();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (active) {
            spriteRenderer.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        SetDefaultColor();
    }

    private void SetDefaultColor() {
        if (captured) {
            spriteRenderer.color = capturedColor;
        } else {
            spriteRenderer.color = active ? activeColor : inactiveColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (active && !clickDisabled) {
            lm.StartLevel();
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

// implements IPointer interfaces so will be properly inactive when UI elemnents overlap it
public class Territory : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public List<Territory> neighbors;
    public LevelManager lm;
    public bool captured;
    public bool active;

    private static readonly Color hoverColor = Color.white;
    private static readonly Color activeColor = new Color(0.8f, 0.8f, 0.8f);
    private static readonly Color inactiveColor = new Color(0.5f, 0.5f, 0.5f);

    public void Start() {
        SetDefaultColor();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (active) {
            this.GetComponent<SpriteRenderer>().color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        SetDefaultColor();
    }

    private void SetDefaultColor() {
        this.GetComponent<SpriteRenderer>().color = active ? activeColor : inactiveColor;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (active) {
            lm.StartLevel();
        }
    }
}

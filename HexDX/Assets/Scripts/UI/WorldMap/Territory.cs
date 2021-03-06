﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

// implements IPointer interfaces so will be properly inactive when UI elemnents overlap it
public class Territory : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    private static readonly Color hoverColor = Color.white;
    private static readonly Color capturedColor = new Color(1f, 0.43f, 0.43f);
    private static readonly Color activeColor = new Color(0.8f, 0.8f, 0.8f);
    private static readonly Color inactiveColor = new Color(0.45f, 0.45f, 0.45f);
    
    public List<Territory> neighbors;
    public LevelManager lm;
    public bool captured;
    public bool active;
    public bool clickDisabled;
    private SpriteRenderer spriteRenderer;
    private bool levelStarted;

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
        if (active && !clickDisabled && !levelStarted) {
            levelStarted = true;
            lm.StartLevel();
        }
    }
}

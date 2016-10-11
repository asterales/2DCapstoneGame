using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

// implements IPointer interfaces so will be properly inactive when UI elemnents overlap it
public class Territory : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public List<Territory> neighbors;
    public LevelManager lm;
    public bool captured;

    public void Start() {
        this.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        this.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData) {
        this.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    public void OnPointerClick(PointerEventData eventData) {
        lm.StartLevel();
    }
}

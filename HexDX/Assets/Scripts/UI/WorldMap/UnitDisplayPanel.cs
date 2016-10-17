using UnityEngine;
using UnityEngine.UI;

public class UnitDisplayPanel : MonoBehaviour {
	private Image unitPanel;
	public Unit unit;

	void Awake() {
		unitPanel = gameObject.GetComponent<Image>();
	}

	void Update() {
		if (unit != null) {
			unitPanel.enabled = true;
			unitPanel.sprite = unit.spriteRenderer.sprite;
		} else {
			unitPanel.enabled = false;
		}
	}
}
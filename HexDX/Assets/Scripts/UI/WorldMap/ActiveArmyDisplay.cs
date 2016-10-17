using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ActiveArmyDisplay : MonoBehaviour {
	private List<UnitDisplayPanel> unitPanels;
	private GameManager gm;

	void Awake() {
		GetUnitPanels();
	}

	void Start() {
		gm = GameManager.instance;
		RefreshDisplay();
	}

	private void GetUnitPanels() {
		unitPanels = new List<UnitDisplayPanel>();
		for(int i = 1; i <= GameManager.UNIT_LIMIT; i++) {
			unitPanels.Add(transform.Find("Unit Panel " + i).GetComponent<UnitDisplayPanel>());
		}
	}

	public void RefreshDisplay() {
		gm.ClearNullUnits();
		for(int i = 0; i < gm.activeUnits.Count; i++) {
			Unit unit = gm.activeUnits[i];
			//unit.gameObject.SetActive(true);
			unit.facing = 0;
			unit.phase = UnitTurn.Open;
			unit.SetFacingSprites();
			unitPanels[i].unit = unit;
		}
	}
}
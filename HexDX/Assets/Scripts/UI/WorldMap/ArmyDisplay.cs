using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class ArmyDisplay : MonoBehaviour {
	protected List<UnitDisplay> unitPanels;

	protected virtual void Awake() {
		GetUnitPanels();
	}

	protected virtual void Start() {
		RefreshDisplay();
	}

	private void GetUnitPanels() {
		unitPanels = new List<UnitDisplay>();
		for(int i = 1; i <= DisplayLimit(); i++) {
			unitPanels.Add(transform.Find("Unit Panel " + i).GetComponent<UnitDisplay>());
		}
	}

	public void RefreshDisplay() {
		List<Unit> units = GetUnitsToDisplay();
		for(int i = 0; i < units.Count; i++) {
			Unit unit = units[i];
			unit.facing = 0;
			unit.phase = UnitTurn.Open;
			unit.SetFacingSprites();
			unitPanels[i].unit = unit;
		}
	}

	protected abstract int DisplayLimit();
	protected abstract List<Unit> GetUnitsToDisplay();
}
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public abstract class ArmyDisplay : MonoBehaviour {
	public List<UnitDisplay> unitPanels;
	public const int DEFAULT_FACING = 5;

	protected virtual void Awake() {
		unitPanels = GetComponentsInChildren<UnitDisplay>().ToList();
	}

	protected virtual void Start() {
		RefreshDisplay();
	}

	public void RefreshDisplay() {
		foreach(UnitDisplay panel in unitPanels) {
			panel.unit = null;
		}
		List<Unit> units = GetUnitsToDisplay();
		for(int i = 0; i < units.Count; i++) {
			Unit unit = units[i];
			unit.facing = DEFAULT_FACING;
			unit.phase = UnitTurn.Open;
			unit.SetFacingSprites();
			unitPanels[i].unit = unit;
		}
	}

	public UnitDisplay GetFirstEmptySlot() {
		return unitPanels.FirstOrDefault(u => u.unit == null);
	}

	protected abstract int DisplayLimit();
	protected abstract List<Unit> GetUnitsToDisplay();
}
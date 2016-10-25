using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public abstract class ArmyDisplay : MonoBehaviour {
	public List<UnitDisplay> unitPanels;

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
			GameManager.SetDefaultUnitView(unit);
			unitPanels[i].unit = unit;
		}
	}

	public UnitDisplay GetFirstEmptySlot() {
		return unitPanels.FirstOrDefault(u => u.unit == null);
	}

	public bool HasUnits() {
		return unitPanels.Where(p => p.unit != null).ToList().Count > 0;
	}

	public List<Unit> GetUnits() {
		return unitPanels.Where(p => p.unit != null).Select(p => p.unit).ToList();
	}

	protected abstract List<Unit> GetUnitsToDisplay();
}
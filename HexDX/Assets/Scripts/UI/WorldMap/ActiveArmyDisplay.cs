using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ActiveArmyDisplay : ArmyDisplay {
	private GameManager gm;

	protected override void Start() {
		gm = GameManager.instance;
		base.Start();
	}

	protected override int DisplayLimit() {
		return GameManager.ACTIVE_UNIT_LIMIT;
	}

	protected override List<Unit> GetUnitsToDisplay() {
		gm.ClearNullUnits();
		return gm.activeUnits;
	}
}
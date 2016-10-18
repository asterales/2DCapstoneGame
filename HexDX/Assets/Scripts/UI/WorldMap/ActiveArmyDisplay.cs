using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ActiveArmyDisplay : ArmyDisplay {

	protected override void Start() {
		base.Start();
	}

	protected override int DisplayLimit() {
		return GameManager.ACTIVE_UNIT_LIMIT;
	}

	protected override List<Unit> GetUnitsToDisplay() {
		GameManager gm = GameManager.instance;
		gm.ClearNullUnits();
		return gm.activeUnits;
	}
}
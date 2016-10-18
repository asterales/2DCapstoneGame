using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class InactiveArmyDisplay : ArmyDisplay {

	protected override void Start() {
		base.Start();
	}

	protected override int DisplayLimit() {
		return GameManager.TOTAL_UNIT_LIMIT;
	}

	protected override List<Unit> GetUnitsToDisplay() {
		List<Unit> inactiveUnits = GameManager.instance.GetInactiveUnits();
		inactiveUnits.ForEach(u => u.gameObject.SetActive(true));
		return inactiveUnits;
	}
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class InactiveArmyDisplay : ArmyDisplay {
	private GameManager gm;

	protected override void Start() {
		gm = GameManager.instance;
		base.Start();
	}

	protected override int DisplayLimit() {
		return GameManager.TOTAL_UNIT_LIMIT;
	}

	protected override List<Unit> GetUnitsToDisplay() {
		List<Unit> inactiveUnits = gm.playerAllUnits.Where(u => !gm.activeUnits.Contains(u)).ToList();
		inactiveUnits.ForEach(u => u.gameObject.SetActive(true));
		return inactiveUnits;
	}
}
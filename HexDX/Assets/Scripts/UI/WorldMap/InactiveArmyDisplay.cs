using System.Collections.Generic;

public class InactiveArmyDisplay : ArmyDisplay {

	protected override void Start() {
		base.Start();
	}
	
	protected override List<Unit> GetUnitsToDisplay() {
		List<Unit> inactiveUnits = GameManager.instance.GetInactiveUnits();
		inactiveUnits.ForEach(u => u.gameObject.SetActive(true));
		return inactiveUnits;
	}
}
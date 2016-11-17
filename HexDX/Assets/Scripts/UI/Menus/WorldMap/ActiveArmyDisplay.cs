using System.Collections.Generic;

public class ActiveArmyDisplay : ArmyDisplay {

	protected override void Start() {
		base.Start();
	}

	protected override List<Unit> GetUnitsToDisplay() {
		GameManager gm = GameManager.instance;
        gm.ClearDeadUnits();
		return gm.activeUnits;
	}
}
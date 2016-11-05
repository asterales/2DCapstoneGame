using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScriptedAIBattleController : ArmyBattleController {
    public List<Unit> aiUnits;

	public override void InitUnits() {
		base.InitUnits();
		PlayerBattleController pbc = BattleControllerManager.instance.player;
		pbc.units = aiUnits.Where(p => p != null && p.IsPlayerUnit()).ToList();
		UnitAI.playerUnits = pbc.units;
	}

    public override void InitUnitList() {
        Unit[] allUnits = FindObjectsOfType(typeof(Unit)) as Unit[];
        units = allUnits.Where(unit => !unit.IsPlayerUnit()).ToList();
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScriptedAIBattleController : ArmyBattleController {
    public List<Unit> aiUnits;

    public override void InitUnitList() {
        Unit[] allUnits = FindObjectsOfType(typeof(Unit)) as Unit[];
        units = allUnits.Where(unit => !unit.IsPlayerUnit()).ToList();
    }
}

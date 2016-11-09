using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScriptedAIBattleController : ArmyBattleController {
    public override void InitUnitList() {
        Unit[] allUnits = BattleControllerManager.instance.hexMap.GetUnitsOnMap();
        units = allUnits.Where(unit => !unit.IsPlayerUnit()).ToList();
    }
}

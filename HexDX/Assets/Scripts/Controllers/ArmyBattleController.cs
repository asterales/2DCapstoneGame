using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class ArmyBattleController : MonoBehaviour {
    public List<Unit> units;
    protected BattleController battleController;

    public abstract void InitUnitList();

    protected virtual void Awake() {
        battleController = GetComponent<BattleController>();
    }

    public virtual void InitUnits() {
        InitUnitList();
        units.ForEach(u => u.InitForBattle());
    }

    public virtual void StartTurn() {
        OpenAllUnits();
    }

    public virtual void EndTurn() {
        OpenAllUnits();
    }

    protected void OpenAllUnits() {
        for (int i = 0; i < units.Count; i++) {
            if (units[i]) {
                units[i].MakeOpen();
            }
        }
    }

    public bool AllUnitsDone() {
        return units.Where(u => u!=null && u.phase != UnitTurn.Done).ToList().Count == 0;
    }

    public bool IsAnnihilated() {
        return units.Where(u => u.enabled || u.Health>0).ToList().Count == 0;
    }

    public bool NoneAttacking() {
    	return units.Where(u => u!=null && u.phase == UnitTurn.Attacking).ToList().Count == 0;
    }
}
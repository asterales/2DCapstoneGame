using UnityEngine;
using System.Collections.Generic;
using MovementEffects;
using System.Linq;
using System;

public class StationaryAI : UnitAI {
	private Unit currentEnemy;
	private bool attackStarted;

	public override void SetMovement() {
		unit.MakeFacing();
	}

    public override void SetFacing() {
        Unit closestEnemy = playerUnits.Where(p => p != null)
                                            .OrderBy(p => HexMap.Cost(p.currentTile, unit.currentTile))
                                            .ToList()[0];
        Vector3 directionVec = closestEnemy.transform.position - unit.transform.position;
        unit.SetFacing(new Vector2(directionVec.x, directionVec.y));
        unit.MakeChoosingAction();
    }

    public override void SetAction() {
        currentEnemy = GetEnemyInRange();
    	if(currentEnemy != null) {
    		attackStarted = false;
    		unit.MakeAttacking();
    	} else {
    		unit.MakeDone();
    	}
    }

    public override void SetAttack() {
    	if (currentEnemy == null) { // in case
    		unit.MakeDone();
    	} else if (!attackStarted) {
            attackStarted = true;
            Timing.RunCoroutine(unit.PerformAttack(currentEnemy));
            SelectionController.ShowTarget(currentEnemy); 
        }
    }

    public override void Reset() { 
    	base.Reset();
    	currentEnemy = null;
    	attackStarted = false;
    }

    public override void Initialize()
    {
        // does nothing
    }
}
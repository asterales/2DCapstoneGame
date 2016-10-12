using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class StationaryAI : UnitAI {
	private Unit currentEnemy;
	private bool attackStarted;

	// Doesn't move
	public override void SetMovement() {
		unit.MakeFacing();
	}

	// Checks each facing direction for enemies, sets facing and currentEnemy if found
    public override void SetFacing() {
        int currentFacing = unit.facing;
    	foreach(Vector2 dir in HexMap.directions) {
    		Vector3 directionVec = HexMap.mapArray[(int)dir.x][(int)dir.y].transform.position - unit.transform.position;
            unit.SetFacing(new Vector2(directionVec.x, directionVec.y));
            currentEnemy = GetEnemyInRange();
            if (currentEnemy != null) {
                break;
            }
    	}
        if (currentEnemy == null) {
            unit.facing = currentFacing; // if no enemies in range, stay current facing
        }
        unit.MakeChoosingAction();
    }

    public override void SetAction() {
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
            StartCoroutine(unit.PerformAttack(currentEnemy));
            SelectionController.ShowTarget(currentEnemy); 
        }
    }

    public override void Reset() { 
    	base.Reset();
    	currentEnemy = null;
    	attackStarted = false;
    }
}
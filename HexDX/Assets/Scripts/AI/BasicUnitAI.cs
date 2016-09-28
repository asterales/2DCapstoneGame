using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BasicUnitAI : UnitAI {
	private Unit currentEnemy;
	private bool attackStarted;

    public override void SetMovement() { 
        Unit nextEnemy = GetEnemyInRange();
        if (nextEnemy == null) { 
            Tile nextDest = GetNextDestination(out nextEnemy);
            unit.SetPath(unit.GetShortestPath(nextDest));   
            unit.MakeMoving(null);           
        } else {
            unit.MakeFacing();
        }
        currentEnemy = nextEnemy;
    }

    public override void SetFacing() {
        if (currentEnemy != null) {
            Vector3 directionVec = currentEnemy.transform.position - unit.transform.position;
            unit.SetFacing(new Vector2(directionVec.x, directionVec.y));
        } else {
            Debug.Log("Enemy not found " + unitNum);
        }
        unit.MakeChoosingAction();
    }

    public override void SetAction() {
    	attackStarted = false;
    	unit.MakeAttacking();
    }

    public override void SetAttack() { 
    	if (!attackStarted) {
            Unit enemy = GetEnemyInRange();
            if (enemy) {
                attackStarted = true;
                StartCoroutine(unit.PerformAttack(enemy));
                SelectionController.ShowTarget(enemy); 
            } else {
                unit.MakeDone();
            }
        }
    }
    
    public override void Reset() { 
    	base.Reset();
    	currentEnemy = null;
    	attackStarted = false;
    }

    private Unit GetEnemyInRange() {
        List<Tile> attackTiles = HexMap.GetAttackTiles(unit);
        return playerUnits.FirstOrDefault(playerUnit => playerUnit != null && attackTiles.Contains(playerUnit.currentTile));
    }

    private Tile GetNextDestination(out Unit nextEnemy) {
        List<Unit> enemiesByDistance = playerUnits.Where(p => p != null).OrderBy(p => unit.GetShortestPath(p.currentTile).Count).ToList();
        List<Tile> validDestinations = HexMap.GetMovementTiles(unit).Where(t => IsValidDestination(t)).ToList();
        if (validDestinations.Count == 0){
            nextEnemy = null;
            return unit.currentTile;
        } else {
            int cost = HexMap.Cost(unit.currentTile, enemiesByDistance[0].currentTile);
            int attackRange = 1; //hard coded attack range
            if (cost < unit.MvtRange + attackRange) {
                validDestinations = validDestinations.Where(t => HexMap.Cost(t, enemiesByDistance[0].currentTile) >= 1).ToList();
            }
            nextEnemy = enemiesByDistance[0];
            return validDestinations.OrderBy(t => HexMap.Cost(t, enemiesByDistance[0].currentTile)).ToList()[0];
        }
    }


    private bool IsValidDestination(Tile tile) {
        return tile != null && tile.pathable && !tile.currentUnit;
    }
}

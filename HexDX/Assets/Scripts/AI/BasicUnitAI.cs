using UnityEngine;
using System.Collections.Generic;
using MovementEffects;
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
                Timing.RunCoroutine(unit.PerformAttack(enemy));
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

    public override void Initialize()
    {
        // does nothing
    }

    private Tile GetNextDestination(out Unit nextEnemy) {
        List<Unit> remainingEnemies = playerUnits.Where(p => p != null).ToList();
        List<Tile> validDestinations = HexMap.GetMovementTiles(unit).Where(t => IsValidDestination(t)).ToList();
        if (validDestinations.Count == 0 || remainingEnemies.Count == 0){
            nextEnemy = null;
            return unit.currentTile;
        }
        // sort enemies by distance and determine next enemy to attack
        List<Unit> enemiesByDistance = remainingEnemies.OrderBy(p => IdaStarDistance(p)).ToList();
        if (IdaStarDistance(enemiesByDistance[0]) == int.MaxValue) {
            // all enemies unreachable, readjust list so destination calculation below will use closest enemy instead
            enemiesByDistance = remainingEnemies.OrderBy(p => HexMap.Cost(unit.currentTile, p.currentTile)).ToList();
            nextEnemy = null;
        } else {
            nextEnemy = enemiesByDistance[0];
        }

        // determine the next destination to move to
        int cost = HexMap.Cost(unit.currentTile, enemiesByDistance[0].currentTile);
        int attackRange = 1; //hard coded attack range
        if (cost < unit.MvtRange + attackRange) {
            validDestinations = validDestinations.Where(t => HexMap.Cost(t, enemiesByDistance[0].currentTile) >= 1).ToList();
        }
        return validDestinations.OrderBy(t => HexMap.Cost(t, enemiesByDistance[0].currentTile)).ToList()[0];
    }

    private int IdaStarDistance(Unit playerUnit) {
        List<Tile> shortestPath = unit.GetShortestPath(playerUnit.currentTile);
        return shortestPath.Count > 0 ? shortestPath.Count : int.MaxValue;  // empty list returned for impossible path
    }

    private bool IsValidDestination(Tile tile) {
        return tile != null && tile.pathable && !tile.currentUnit;
    }
}

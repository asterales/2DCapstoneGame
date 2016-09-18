using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BasicUnitAI : UnitAI {
	private Unit currentEnemy;
	private bool attackStarted;

    public override void SetMovement() { 
        Unit nextEnemy = GetEnemyInRange(unit);
        if (nextEnemy == null) { 
            List<Tile> shortestRoute = null;
            foreach(Unit enemy in playerUnits) {
                if (enemy) {
                    List<Tile> route = unit.GetShortestPath(enemy.currentTile);
                    if(route[route.Count - 1] = enemy.currentTile) {
                        route.RemoveRange(route.Count - 2, 2); //CHANGE LATER: hard coded hack to put player unit in attack range
                        Debug.Log("Shortened path to " + route.Count + unitNum);
                    }
                    if (shortestRoute == null || route.Count < shortestRoute.Count) {
                        shortestRoute = route;
                        nextEnemy = enemy;
                        Debug.Log("Set a route " + unitNum);
                    }
                }
            }
            if (shortestRoute != null) {
                Debug.Log("Made moving + " + unitNum);
                unit.SetPath(shortestRoute);   
                unit.MakeMoving(); 
            } else {
                Debug.Log("Route null so facing + " + unitNum);
                unit.MakeFacing();
            }            
        } else {
            Debug.Log("Has an enemy so facing + " + unitNum);
            unit.MakeFacing();
        }
        currentEnemy = nextEnemy;
        Debug.Log("next enemy set + " + unitNum);
    }

    public override void SetFacing() {
        Debug.Log ("AI Facing"+ unitNum);
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
            Debug.Log ("AI Attacking"+ unitNum);
            List<Tile> attackTiles = HexMap.GetAttackTiles(unit.currentTile);
            Unit enemy = playerUnits.FirstOrDefault(playerUnit => attackTiles.Contains(playerUnit.currentTile));
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
    	Debug.Log ("Resetting "+ unitNum);
    	currentEnemy = null;
    	attackStarted = false;
    }

    public Unit GetEnemyInRange(Unit unit) {
        List<Tile> attackTiles = HexMap.GetAttackTiles(unit.currentTile);
        foreach (Unit enemy in playerUnits) {
            if (enemy != null && attackTiles.Contains(enemy.currentTile)) {
                return enemy;
            }
        }
        return null;
    }
}

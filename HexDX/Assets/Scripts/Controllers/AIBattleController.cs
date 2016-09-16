using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AIBattleController : MonoBehaviour {
    private BattleController battleController;
    private List<Unit> units;
    private List<Unit> playerUnits;

    //keeping track of last unit being modified last update
    private int currentUnitIndex;
    private bool[] attackStarted;
    private Unit[] currentEnemy;

    void Start() {
        InitUnitList();
        battleController = gameObject.GetComponent<BattleController>();
        playerUnits = gameObject.GetComponent<PlayerBattleController>().units;
        attackStarted = new bool[units.Count];
        currentEnemy = new Unit[units.Count];
    }

    private void InitUnitList() {
        Unit[] allUnits = FindObjectsOfType(typeof(Unit)) as Unit[];
        units = allUnits.Where(unit => !unit.isPlayerUnit).ToList();
    }

    void Update() {
        if (SelectionController.TakingAIInput()){
            if (currentUnitIndex < units.Count) {
                if (units[currentUnitIndex]) {
                    Debug.Log(currentUnitIndex);
                    switch(units[currentUnitIndex].phase) {
                        case UnitTurn.Open:
                            SetPathToClosestEnemy();
                            break;
                        case UnitTurn.Facing:
                            SetFacing();
                            break;
                        case UnitTurn.Attacking:
                            AttackEnemyInRange();
                            break;
                        case UnitTurn.Done:
                            ResetUnit();
                            currentUnitIndex++;
                            break;
                        default:
                            break;
                    }
                } else {
                    currentUnitIndex++;
                } 
            } else {
                battleController.EndCurrentTurn();
            }
        }
    }

    private void SetPathToClosestEnemy() {
        Unit unit = units[currentUnitIndex];
        Unit nextEnemy = GetEnemyInRange(unit);
        if (nextEnemy == null) { 
            List<Tile> shortestRoute = null;
            foreach(Unit enemy in playerUnits) {
                if (enemy) {
                    List<Tile> route = unit.GetShortestPath(enemy.currentTile);
                    if(route[route.Count - 1] = enemy.currentTile) {
                        route.RemoveRange(route.Count - 2, 2); //CHANGE LATER: hard coded hack to put player unit in attack range
                        //Debug.Log("Shortened path to " + route.Count + currentUnitIndex);
                    }
                    if (shortestRoute == null || route.Count < shortestRoute.Count) {
                        shortestRoute = route;
                        nextEnemy = enemy;
                        //Debug.Log("Set a route " + currentUnitIndex);
                    }
                }
            }
            if (shortestRoute != null) {
                //Debug.Log("Made moving + " + currentUnitIndex);
                unit.SetPath(shortestRoute);   
                unit.MakeMoving(); 
            } else {
                //Debug.Log("Route null so facing + " + currentUnitIndex);
                unit.MakeFacing();
            }            
        } else {
            //Debug.Log("Has an enemy so facing + " + currentUnitIndex);
            unit.MakeFacing();
        }
        currentEnemy[currentUnitIndex] = nextEnemy;
        //Debug.Log("next enemy set + " + currentUnitIndex);
    }

    private void SetFacing() {
        //Debug.Log ("AI Facing"+ currentUnitIndex);
        Unit unit = units[currentUnitIndex];
        Unit enemy = currentEnemy[currentUnitIndex];
        if (enemy != null) {
            Vector3 directionVec = enemy.transform.position - unit.transform.position;
            unit.SetFacing(new Vector2(directionVec.x, directionVec.y));
        } else {
            Debug.Log("Enemy not found " + currentUnitIndex);
        }
        unit.MakeAttacking();
        attackStarted[currentUnitIndex] = false;
    }

    private void AttackEnemyInRange() {
        if (!attackStarted[currentUnitIndex]) {
            //Debug.Log ("AI Attacking"+ currentUnitIndex);
            Unit unit = units[currentUnitIndex];
            List<Tile> attackTiles = HexMap.GetAttackTiles(unit.currentTile);
            Unit enemy = playerUnits.FirstOrDefault(playerUnit => attackTiles.Contains(playerUnit.currentTile));
            if (enemy) {
                attackStarted[currentUnitIndex] = true;
                StartCoroutine(unit.PerformAttack(enemy));
                SelectionController.ShowTarget(enemy); 
            } else {
                unit.MakeDone();
            }
        }
    }

    private void ResetUnit() {
        Debug.Log ("Resetting "+ currentUnitIndex);
        attackStarted[currentUnitIndex] = false;
        currentEnemy[currentUnitIndex] = null;
        SelectionController.HideTarget();
    }

    public void StartTurn() {
        currentUnitIndex = 0;
        SelectionController.mode = SelectionMode.AITurn;
    }

    public void EndTurn() {
        for (int i = 0; i < units.Count; i++) {
            if(units[i]) {
                units[i].MakeOpen();
            }
        }
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

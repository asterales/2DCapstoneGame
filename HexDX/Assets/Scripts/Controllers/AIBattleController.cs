using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AIBattleController : MonoBehaviour {
    private BattleController battleController;
    public List<Unit> units;
    private List<Unit> playerUnits;

    //keeping track of last unit being modified last update
    private int currentUnitIndex;

    void Start() {
        battleController = gameObject.GetComponent<BattleController>();
        playerUnits = gameObject.GetComponent<PlayerBattleController>().units;
    }

    void Update() {
        if (SelectionController.TakingAIInput()) {
            if (currentUnitIndex < units.Count) {
                Unit currentUnit = units[currentUnitIndex];
                switch(currentUnit.phase) {
                    case UnitTurn.Open:
                        SetPathToClosestEnemy(currentUnit);
                        break;
                    case UnitTurn.Facing:
                        currentUnit.MakeDone();
                        break;
                    case UnitTurn.Done:
                        currentUnitIndex++;
                        break;
                    default:
                        break;
                }
            } else {
                battleController.EndCurrentTurn();
            }
        }
    }

    public void StartTurn() {
        currentUnitIndex = 0;
        SelectionController.selectionMode = SelectionMode.AITurn;
    }

    private void SetPathToClosestEnemy(Unit unit) {
        if (!unit.HasEnemyInRange()) { 
            List<Tile> shortestRoute = null;
            foreach(Unit enemy in playerUnits) {
                List<Tile> enemyNeighborTiles = HexMap.GetNeighbors(enemy.currentTile);
                foreach (Tile neighbor in enemyNeighborTiles) {
                    if (neighbor && neighbor.currentUnit == null) {
                        List<Tile> route = unit.GetShortestPath(neighbor);
                        if (shortestRoute == null || route.Count < shortestRoute.Count) {
                            shortestRoute = route;
                        }
                    }
                }
            }
            unit.SetPath(shortestRoute);
            unit.MakeMoving();            
        } else {
            unit.MakeFacing();
        }
    }

    public void EndTurn() {
        for (int i = 0; i < units.Count; i++) {
            units[i].MakeOpen();
        }
    }
}

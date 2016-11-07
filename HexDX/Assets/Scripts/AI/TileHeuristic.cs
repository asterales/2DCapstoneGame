// Heuristic used for deciding move
// Contains Heuristic for deciding face and attacking unit

using System.Collections.Generic;
using UnityEngine;

public class TileHeuristic : Heuristic
{
    public List<Unit> enemyUnits;
    public List<Objective> objectives;
    public float statBoost;        // stat boost from tile (0.0 <= v <= 1.0)
    public float distToClosestEnemy; // distance to closest enemy (0.0 <= v <= 1.0)
    public float distToObjective;    // distance to closest objective (0.0 <= v <= 1.0)
    public bool hasMoved;

    public TileHeuristic()
    {
        tile = null;
        unit = null;
        closestEnemyUnit = null;
        closestObjective = null;
        hasMoved = true;
    }

    public TileHeuristic(Tile t, Unit u, List<Unit> enemies, List<Objective> obj)
    {
        tile = t;
        unit = u;
        enemyUnits = enemies;
        objectives = obj;
        hasMoved = true;
    }

    public override void EvaluateData()
    {
        CalculateStatBoost();
        CalculateObjectiveDist();
        CalculateEnemyDist();
        hasMoved = tile != unit.currentTile;
    }

    private void CalculateStatBoost()
    {
        // return the stat boost of the tile
        statBoost = 0.0f;
    }

    private void CalculateEnemyDist()
    {
        // calculate distance to nearest enemy (1.0 if right next to ai 1.0 / dist otherwise)
        float minDistance = 1110.0f;
        for (int i = 0; i < enemyUnits.Count; i++)
        {
            float dist = (float)IdaStarDistance(enemyUnits[i]);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestEnemyUnit = enemyUnits[i];
            }
        }
        distToClosestEnemy = 1.0f / minDistance;
    }

    private void CalculateObjectiveDist()
    {
        // calculate distance to nearest objective (1.0 if right next to ai 1.0 / dist otherwise)
        if (objectives == null)
        {
            distToObjective = 0.0f;
            return;
        }
        float minDistance = 0.0f;
        for (int i=0;i<objectives.Count;i++)
        {
            float dist = (float)IdaStarDistance(objectives[i]);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestObjective = objectives[i];
            }
        }
        distToObjective = minDistance;
    }

    public override float CalculateHeuristic(AIWeights weights)
    {
        float heuristic = 0.0f;
        heuristic += statBoost * weights.tileStatBoost;
        heuristic += distToClosestEnemy * weights.tileEnemyCloseness;
        heuristic += (1.0f - distToClosestEnemy) * weights.tileEnemyDistance;
        heuristic += distToObjective * weights.tileClosenessObjective;
        if (!hasMoved) heuristic += weights.tileRefrain;
        heuristic *= weights.tileGlobal;
        return heuristic;
    }

    private int IdaStarDistance(Unit playerUnit)
    {
        int shortestPathLength = unit.GetShortestPathLength(playerUnit.currentTile, tile);
        return shortestPathLength;
    }

    private int IdaStarDistance(Objective objective)
    {
        int shortestPathLength = unit.GetShortestPathLength(objective.location, tile);
        return shortestPathLength;
    }
}

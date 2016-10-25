// Heuristic used for deciding move
// Contains Heuristic for deciding face and attacking unit

using System.Collections.Generic;

public class TileHeuristic : Heuristic
{
    public Tile tileToMoveTo;
    public List<Unit> enemyUnits;
    public List<Objective> objectives;
    public float statBoost;        // stat boost from tile (0.0 <= v <= 1.0)
    public float distToClosestEnemy; // distance to closest enemy (0.0 <= v <= 1.0)
    public float distToObjective;    // distance to closest objective (0.0 <= v <= 1.0)

    public TileHeuristic()
    {
        tileToMoveTo = null;
    }

    public TileHeuristic(Tile tile, List<Unit> enemies, List<Objective> obj)
    {
        tileToMoveTo = tile;
        enemyUnits = enemies;
        objectives = obj;
    }

    public override void EvaluateData()
    {
        CalculateStatBoost();
        CalculateObjectiveDist();
        CalculateEnemyDist();
    }

    private void CalculateStatBoost()
    {
        // return the stat boost of the tile
        statBoost = 0.0f;
    }

    private void CalculateEnemyDist()
    {
        // calculate distance to nearest enemy (1.0 if right next to ai 1.0 / dist otherwise)
        float minDistance = 0.0f;
        for (int i = 0; i < objectives.Count; i++)
        {
            // to be implemented
        }
        distToClosestEnemy = minDistance;
    }

    private void CalculateObjectiveDist()
    {
        // calculate distance to nearest objective (1.0 if right next to ai 1.0 / dist otherwise)
        float minDistance = 0.0f;
        for (int i=0;i<objectives.Count;i++)
        {
            // to be implemented
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
        heuristic *= weights.tileGlobal;
        return heuristic;
    }
}

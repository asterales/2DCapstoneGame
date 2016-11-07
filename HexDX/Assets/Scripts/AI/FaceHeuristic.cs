using UnityEngine;
using System.Collections.Generic;
using System;

// this one will be difficult

public class FaceHeuristic : Heuristic {
    public List<Unit> unitsInRange;
    public int direction;
    public float flankAmount;
    public float directAmount;
    public float sneakAmount;
    public float stateComparisons;
    public float faceClosestUnit;
    public float faceClosestObjective;
    public bool getsKilled;

    public FaceHeuristic()
    {
        unitsInRange = null;
        tile = null;
        unit = null;
        direction = -1;
    }

    public FaceHeuristic(List<Unit> enemies, Tile t, Unit u, int dir)
    {
        unitsInRange = enemies;
        tile = t;
        unit = u;
        direction = dir;
    }

    public override void EvaluateData()
    {
        flankAmount = 0.0f;
        directAmount = 0.0f;
        sneakAmount = 0.0f;
        stateComparisons = 0.0f;
        getsKilled = false;
        for (int i=0;i<unitsInRange.Count;i++)
        {
            CalculateAttackForUnit(unitsInRange[i]);
            CalculateStateDifferencesForUnit(unitsInRange[i]);
        }
        CalculateFacing();
    }

    private void CalculateAttackForUnit(Unit unit)
    {
        // check if unit can hit you
        // check if unit can flank you
        // check if unit can sneak you
        // check if they can kill
        // to be implemented
        if (false)
        {
            flankAmount += 1.0f;
            directAmount += 1.0f;
            sneakAmount += 1.0f;
        }
    }

    private void CalculateStateDifferencesForUnit(Unit unit)
    {
        // to be implemented
        // if unit can hit you
        // check if relatively stronger
        // check if they have more health
        if (false)
        {
            stateComparisons += 1.0f;
            getsKilled = false;
        }
    }

    private void CalculateFacing()
    {
        faceClosestUnit = 0.0f;
        if (closestEnemyUnit != null)
        {
            Vector3 directionOfDirectAttack = closestEnemyUnit.currentTile.transform.position - tile.transform.position;
            int directFace = Facing(new Vector2(directionOfDirectAttack.x, directionOfDirectAttack.y));
            faceClosestUnit = 1.0f - FaceComparison(direction, directFace);
        }

        faceClosestObjective = 0.0f;
        if (closestObjective != null)
        {
            Vector3 directionOfDirectAttack = closestEnemyUnit.currentTile.transform.position - tile.transform.position;
            int directFace = Facing(new Vector2(directionOfDirectAttack.x, directionOfDirectAttack.y));
            faceClosestObjective = 1.0f - FaceComparison(direction, directFace);
        }
    }

    public override float CalculateHeuristic(AIWeights weights)
    {
        float heuristic = 0.0f;
        heuristic += stateComparisons * weights.faceStateComparison;
        heuristic -= directAmount * weights.faceAttackDisadvantage;
        heuristic -= flankAmount * weights.faceFlankDisadvantage;
        heuristic -= sneakAmount * weights.faceSneakDisadvantage;
        if (getsKilled) heuristic -= weights.faceDeathDisadvantage;
        if (closestEnemyUnit) heuristic += faceClosestUnit * weights.faceClosestEnemy;
        if (closestObjective) heuristic += faceClosestObjective * weights.faceClosestObjective;
        heuristic *= weights.faceGlobal;
        return heuristic;
    }
}

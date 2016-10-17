using UnityEngine;
using System.Collections.Generic;
using System;

public class FaceHeuristic : Heuristic {
    public List<Unit> unitsInRange;
    public Tile tilePosition;
    public int direction;
    public float flankAmount;
    public float directAmount;
    public float sneakAmount;
    public float stateComparisons;
    public bool getsKilled;

    public FaceHeuristic()
    {
        unitsInRange = null;
        tilePosition = null;
        direction = -1;
    }

    public FaceHeuristic(List<Unit> enemies, Tile tile, int dir)
    {
        unitsInRange = enemies;
        tilePosition = tile;
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
    }

    private void CalculateAttackForUnit(Unit unit)
    {
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
        if (false)
        {
            stateComparisons += 1.0f;
            getsKilled = false;
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
        heuristic *= weights.faceGlobal;
        return heuristic;
    }
}

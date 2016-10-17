using UnityEngine;
using System.Collections.Generic;
using System;

public class FaceOption : AIOption
{
    public Tile chosenTile;
    public List<AttackOption> attackOptions;
    public FaceHeuristic heuristic;

    public FaceOption()
    {
        chosenTile = null;
        attackOptions = new List<AttackOption>();
        heuristic = null;
    }

    public FaceOption(Tile tile)
    {
        chosenTile = tile;
        attackOptions = new List<AttackOption>();
        heuristic = null;
    }

    public override void LoadOptionData()
    {
        // to be implemented
    }

    public override void EvaluateOptionData()
    {
        // to be implemented
    }
}

// Ignore implementation for now. Currently in the process of refactoring

/*public class FaceOption
{
    public List<UnitFaceHeuristic> unitFaceHeuristics;
    public int direction;
    public int weight;

    public FaceOption(int dir, int w)
    {
        unitFaceHeuristics = new List<UnitFaceHeuristic>();
        direction = dir;
        weight = w;
    }

    public void GetUnitFaceHeuristics(Tile tile, List<Unit> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            int result = units[i].CanReachTileAndAttack(tile, direction);
            if (result > 0)
            {
                unitFaceHeuristics.Add(new UnitFaceHeuristic(units[i], result));
            }
        }
    }
}*/

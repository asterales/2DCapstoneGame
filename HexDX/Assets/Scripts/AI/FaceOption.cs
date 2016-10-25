using UnityEngine;
using System.Collections.Generic;
using System;

public class FaceOption : AIOption
{
    public Tile chosenTile;
    public List<AttackOption> attackOptions;
    public FaceHeuristic heuristic;
    public int direction;
    public float weight;

    public FaceOption()
    {
        chosenTile = null;
        direction = -1;
        attackOptions = new List<AttackOption>();
        heuristic = null;
    }

    public FaceOption(Tile tile, int dir)
    {
        chosenTile = tile;
        direction = dir;
        attackOptions = new List<AttackOption>();
        heuristic = null;
    }

    public override void LoadOptionData()
    {
        heuristic.EvaluateData();
        for (int i = 0; i < attackOptions.Count; i++)
        {
            attackOptions[i].LoadOptionData();
        }
    }

    public override void EvaluateOptionData(AIWeights weights)
    {
        weight = heuristic.CalculateHeuristic(weights);
    }
}


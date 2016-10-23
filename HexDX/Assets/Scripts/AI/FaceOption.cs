using UnityEngine;
using System.Collections.Generic;
using System;

public class FaceOption : AIOption
{
    public Tile chosenTile;
    public List<AttackOption> attackOptions;
    public FaceHeuristic heuristic;
    public float weight;

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
        heuristic.EvaluateData();
    }

    public override void EvaluateOptionData(AIWeights weights)
    {
        weight = heuristic.CalculateHeuristic(weights);
    }
}


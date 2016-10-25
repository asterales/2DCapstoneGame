// Option used for deciding move
// Contains Heuristic for deciding face and attacking unit
// Temporary relocation of old logic while i make a better AI structure

using System;
using System.Collections.Generic;

public class TileOption : AIOption
{
    public Tile chosenTile;
    public List<FaceOption> faceOptions;
    public TileHeuristic heuristic;
    public float weight;

    public TileOption()
    {
        chosenTile = null;
        faceOptions = new List<FaceOption>();
        heuristic = null;
    }

    public TileOption(Tile tile)
    {
        chosenTile = tile;
        faceOptions = new List<FaceOption>();
        heuristic = null;
    }

    public override void LoadOptionData()
    {
        heuristic.EvaluateData();
        for (int i = 0; i < faceOptions.Count; i++)
        {
            faceOptions[i].LoadOptionData();
        }
    }

    public override void EvaluateOptionData(AIWeights weights)
    {
        weight = heuristic.CalculateHeuristic(weights);
    }
}
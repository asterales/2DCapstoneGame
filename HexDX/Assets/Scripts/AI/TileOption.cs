// Option used for deciding move
// Contains Heuristic for deciding face and attacking unit

using System.Collections.Generic;

public class TileOption : AIOption
{
    public Tile chosenTile;
    public List<FaceOption> faceOptions;
    public FaceOption bestFaceOption;
    public TileHeuristic heuristic;
    public float weight;

    public TileOption()
    {
        chosenTile = null;
        faceOptions = new List<FaceOption>();
        heuristic = null;
        bestFaceOption = null;
    }

    public TileOption(Tile tile)
    {
        chosenTile = tile;
        faceOptions = new List<FaceOption>();
        heuristic = null;
        bestFaceOption = null;
    }

    public override void LoadOptionData()
    {
        heuristic.EvaluateData();
        for (int i = 0; i < faceOptions.Count; i++)
        {
            faceOptions[i].heuristic.closestEnemyUnit = heuristic.closestEnemyUnit;
            faceOptions[i].heuristic.closestObjective = heuristic.closestObjective;
            faceOptions[i].LoadOptionData();
        }
    }

    public override void EvaluateOptionData(AIWeights weights)
    {
        weight = heuristic.CalculateHeuristic(weights);
        for (int i = 0; i < faceOptions.Count; i++)
        {
            faceOptions[i].EvaluateOptionData(weights);
            if (bestFaceOption == null || bestFaceOption.weight < faceOptions[i].weight)
            {
                bestFaceOption = faceOptions[i];
            }
        }
    }
}
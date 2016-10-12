using UnityEngine;
using System.Collections.Generic;

// base class for heuristic based AIs
// heuristic based AIs assign a weight to every possible tile that they can move to
// tile with most weight is the decided move

public abstract class HeuristicAI : UnitAI {
    public List<TileHeuristic> tileWeights;

    public void PerformAIAction()
    {
        tileWeights = new List<TileHeuristic>();
        AIChoice choice = DecideAIAction();
        SetMovement(choice);
        SetFacing(choice);
        SetAttack(choice);
        tileWeights.Clear();
    }

    public void CalculateAIHeuristics()
    {
        // initialize tile heuristics
        CreateTileHeuristics();

        // initialize all  other heuristics
        for (int i = 0; i < tileWeights.Count; i++)
        {
            tileWeights[i].InitializeFaceHeuristics();
            tileWeights[i].InitializeUnitHeuristics(playerUnits);
            for(int j=0;j<tileWeights[i].faceHeuristics.Count;j++)
            {
                tileWeights[i].faceHeuristics[j].GetUnitFaceHeuristics(tileWeights[i].tile, playerUnits);
            }
        }

        // TODO :: the order of heuristic calculation may seem incorrect. Currently I am calculating the facing
        //   before deciding which unit to attack, which could lead to incorrect facing for a given attack or 
        //   vice versa. Need to fix that but I am not going to now

        // calculate heuristics for each tile
        for (int i = 0; i < tileWeights.Count; i++)
        {
            for (int j = 0; j < tileWeights[i].faceHeuristics.Count; j++)
            {
                CalculateFaceHeuristic(tileWeights[i].faceHeuristics[j]);
            }
            for (int j = 0; j < tileWeights[i].unitHeuristics.Count; j++)
            {
                CalculateUnitHeuristic(tileWeights[i].unitHeuristics[j]);
            }
            CalculateTileHeuristic(tileWeights[i]);
        }
    }

    private void CreateTileHeuristics()
    {
        // create a tile heuristic for every movable tile in range
    }

    // individual heuristic calculations differ between AIs
    public abstract void CalculateTileHeuristic(TileHeuristic tile);
    public abstract void CalculateFaceHeuristic(FaceHeuristic face);
    public abstract void CalculateUnitHeuristic(UnitHeuristic unit);

    public AIChoice DecideAIAction()
    {
        CalculateAIHeuristics();
        AIChoice choice = new AIChoice();
        choice.tileChoice = FindBestMoveChoice(choice);
        choice.faceChoice = FindBestFaceChoice(choice);
        choice.unitChoice = FindBestUnitChoice(choice);
        return null;
    }

    public TileHeuristic FindBestMoveChoice(AIChoice choice)
    {
        List<TileHeuristic> tiles = tileWeights;
        TileHeuristic bestChoice = null;
        int bestWeight = -1000000;
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].weight > bestWeight)
            {
                bestWeight = tiles[i].weight;
                bestChoice = tiles[i];
            }
        }
        return bestChoice;
    }

    public FaceHeuristic FindBestFaceChoice(AIChoice choice)
    {
        List<FaceHeuristic> faces = choice.tileChoice.faceHeuristics;
        FaceHeuristic bestChoice = null;
        int bestWeight = -1000000;
        for (int i = 0; i < faces.Count; i++)
        {
            if (faces[i].weight > bestWeight)
            {
                bestWeight = faces[i].weight;
                bestChoice = faces[i];
            }
        }
        return bestChoice;
    }

    public UnitHeuristic FindBestUnitChoice(AIChoice choice)
    {
        List<UnitHeuristic> units = choice.tileChoice.unitHeuristics;
        UnitHeuristic bestChoice = null;
        int bestWeight = -1000000;
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].weight > bestWeight)
            {
                bestWeight = units[i].weight;
                bestChoice = units[i];
            }
        }
        return bestChoice;
    }

    public void SetMovement(AIChoice choice)
    {
        // to be implemented
    }

    public void SetFacing(AIChoice choice)
    {
        // to be implemented
    }

    public void SetAction(AIChoice choice)
    {
        // I dont think this is needed since the action will be done if no
        // unit attack choice is made
    }

    public void SetAttack(AIChoice choice)
    {
        if (choice.unitChoice == null)
        {
            // put this AI in Done state
        }
        // to be implemented
    }

    public override void SetMovement() { }
    public override void SetFacing() { }
    public override void SetAction() { }
    public override void SetAttack() { }
}

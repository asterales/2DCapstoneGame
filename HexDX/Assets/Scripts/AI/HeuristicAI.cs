using UnityEngine;
using System.Collections.Generic;

// base class for heuristic based AIs
// heuristic based AIs assign a weight to every possible tile that they can move to
// tile with most weight is the decided move

// TEMPORARILY COMMENTING OUT WHILE I FIX THE STRUCTURE OF THE AI

/*public abstract class HeuristicAI : UnitAI {
    public List<TileHeuristic> tileWHeuristics;
    public AIWeights aiWeights;

    public void PerformAIAction()
    {
        tileWHeuristics = new List<TileHeuristic>();
        AIChoice choice = DecideAIAction();
        SetMovement(choice);
        SetFacing(choice);
        SetAttack(choice);
        tileWHeuristics.Clear();
    }

    public void CalculateAIHeuristics()
    {
        // initialize tile heuristics
        CreateTileHeuristics();

        // initialize all  other heuristics
        for (int i = 0; i < tileWHeuristics.Count; i++)
        {
            tileWHeuristics[i].InitializeFaceHeuristics();
            tileWHeuristics[i].InitializeUnitHeuristics(playerUnits);
            for(int j=0;j< tileWHeuristics[i].faceHeuristics.Count;j++)
            {
                tileWHeuristics[i].faceHeuristics[j].GetUnitFaceHeuristics(tileWHeuristics[i].tile, playerUnits);
            }
        }

        // TODO :: the order of heuristic calculation may seem incorrect. Currently I am calculating the facing
        //   before deciding which unit to attack, which could lead to incorrect facing for a given attack or 
        //   vice versa. Need to fix that but I am not going to now

        // calculate heuristics for each tile
        for (int i = 0; i < tileWHeuristics.Count; i++)
        {
            for (int j = 0; j < tileWHeuristics[i].faceHeuristics.Count; j++)
            {
                CalculateFaceHeuristic(tileWHeuristics[i].faceHeuristics[j]);
            }
            for (int j = 0; j < tileWHeuristics[i].unitHeuristics.Count; j++)
            {
                CalculateUnitHeuristic(tileWHeuristics[i].unitHeuristics[j]);
            }
            CalculateTileHeuristic(tileWHeuristics[i]);
        }
    }

    private void CreateTileHeuristics()
    {
        // create a tile heuristic for every movable tile in range
    }

    // individual heuristic calculations differ between AIs
    public void CalculateTileHeuristic(TileHeuristic tile)
    {
        // to be implemented
    }

    public void CalculateFaceHeuristic(FaceHeuristic face)
    {
        // to be implemented
    }

    public void CalculateUnitHeuristic(UnitHeuristic unit)
    {
        // to be implemented
    }

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
        List<TileHeuristic> tiles = tileWHeuristics;
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

    // probably not needed
    public void SetMovement(AIChoice choice)
    {
        // to be implemented
    }

    // probably not needed
    public void SetFacing(AIChoice choice)
    {
        // to be implemented
    }

    // probably not needed
    public void SetAction(AIChoice choice)
    {
        // I dont think this is needed since the action will be done if no
        // unit attack choice is made
    }

    // probably not needed
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
*/
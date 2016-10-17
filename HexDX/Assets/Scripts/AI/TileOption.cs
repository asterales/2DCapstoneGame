﻿// Option used for deciding move
// Contains Heuristic for deciding face and attacking unit
// Temporary relocation of old logic while i make a better AI structure

using System.Collections.Generic;

// Ignore implementation for now. Currently in the process of refactoring

public class TileOption
{
    public List<AttackOption> unitHeuristics;
    public List<FaceOption> faceHeuristics;
    public FaceHeuristic bestFace;
    public AttackOption bestUnit;
    public Tile tile;
    public int weight;

    public TileOption()
    {
        unitHeuristics = new List<AttackOption>();
        faceHeuristics = new List<FaceOption>();
        tile = null;
        weight = -1000000;
        bestFace = null;
        bestUnit = null;
    }

    public TileOption(Tile t, int w)
    {
        unitHeuristics = new List<AttackOption>();
        faceHeuristics = new List<FaceOption>();
        tile = t;
        weight = w;
        bestFace = null;
        bestUnit = null;
    }

    public void InitializeUnitHeuristics(List<Unit> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            //if ()
            // to be implemented
        }
    }

    public void InitializeFaceHeuristics()
    {
        for (int i = 0; i < 6; i++)
        {
            faceHeuristics.Add(new FaceOption(i, -1000000));
        }
    }

    public float CalculateHeuristic()
    {
        ChooseBestCombination();
        // CONTINUE FROM HERE
        // to be implemented
        return 0.0f;
    }

    public void ChooseBestCombination()
    {
        // to be implemented
        bestFace = null;
        bestUnit = null;
    }
}

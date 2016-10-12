// Heuristic used for deciding move
// Contains Heuristic for deciding face and attacking unit

using System.Collections.Generic;

public class TileHeuristic {
    public List<UnitHeuristic> unitHeuristics;
    public List<FaceHeuristic> faceHeuristics;
    public FaceHeuristic bestFace;
    public UnitHeuristic bestUnit;
    public Tile tile;
    public int weight;

    public TileHeuristic()
    {
        unitHeuristics = new List<UnitHeuristic>();
        faceHeuristics = new List<FaceHeuristic>();
        tile = null;
        weight = -1000000;
        bestFace = null;
        bestUnit = null;
    }

    public TileHeuristic(Tile t, int w)
    {
        unitHeuristics = new List<UnitHeuristic>();
        faceHeuristics = new List<FaceHeuristic>();
        tile = t;
        weight = w;
        bestFace = null;
        bestUnit = null;
    }

    public void InitializeUnitHeuristics(List<Unit> units)
    {
        for(int i=0;i<units.Count;i++)
        {
            //if ()
            // to be implemented
        }
    }

    public void InitializeFaceHeuristics()
    {
        for(int i=0;i<6;i++)
        {
            faceHeuristics.Add(new FaceHeuristic(i, -1000000));
        }
    }

    public void ChooseBestUnit()
    {
        // to be implemented
    }

    public void ChooseBestFace()
    {
        // to be implemented
    }
}

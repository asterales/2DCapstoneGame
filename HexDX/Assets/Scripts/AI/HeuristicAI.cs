using UnityEngine;
using System.Collections.Generic;
using System;

// base class for heuristic based AIs
// heuristic based AIs assign a weight to every possible tile that they can move to
// tile with most weight is the decided move

public class HeuristicAI : UnitAI
{
    // UnitAI's variables for reference
    //protected static List<Unit> playerUnits;
    //public Unit unit;
    //public int unitNum;
    public List<TileOption> tileOptions;
    public AIWeights weightFunction;
    private AIChoice choice;

    void Start()
    {
        tileOptions = new List<TileOption>();
    }

    public void MakeChoice(List<Tile> ignore)
    {
        choice = null;
        // make best choice based on the tiles and ignoring all already chosen tiles
        // to be implemented
        choice = new AIChoice(null, null, null);
    }

    // run every turn to give the AI the info it needs
    public void LoadDataForAI()
    {
        for (int i=0;i<tileOptions.Count;i++)
        {
            tileOptions[i].LoadOptionData();
        }
    }

    // run every turn to evaluate the AI data
    public void EvaluateDataForAI()
    {
        for (int i=0;i<tileOptions.Count;i++)
        {
            tileOptions[i].EvaluateOptionData(weightFunction);
        }
    }

    public override void SetAction()
    {
        // to be implemented
    }

    public override void SetAttack()
    {
        // to be implemented
    }

    public override void SetFacing()
    {
        // to be implemented
    }

    public override void SetMovement()
    {
        // to be implemented
    }
}
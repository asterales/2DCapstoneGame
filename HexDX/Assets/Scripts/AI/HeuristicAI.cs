using UnityEngine;
using System.Collections.Generic;
using MovementEffects;
using System;

// base class for heuristic based AIs
// heuristic based AIs assign a weight to every possible tile that they can move to
// tile with most weight is the decided move

public class HeuristicAI : UnitAI
{
    public List<TileOption> tileOptions;
    public AIWeights weightFunction;
    public AIChoice choice;

    void Start()
    {
        tileOptions = new List<TileOption>();
    }

    // this is inefficient just making it fast for now
    public void MakeChoice(List<Tile> ignore)
    {
        choice = new AIChoice(null, null, null);
        for (int i = 0; i < tileOptions.Count; i++)
        {
            if (ignore.Contains(tileOptions[i].chosenTile)) continue;
            TileOption tileChoice = tileOptions[i];
            FaceOption faceChoice = null;
            AttackOption attackChoice = null;
            for (int j = 0; j < tileOptions[i].faceOptions.Count; j++)
            {
                faceChoice = tileOptions[i].faceOptions[j];
                attackChoice = null;
                if (tileOptions[i].faceOptions[j].attackOptions.Count > 0)
                {
                    for (int k = 0; k < tileOptions[i].faceOptions[j].attackOptions.Count; k++)
                    {
                        attackChoice = tileOptions[i].faceOptions[j].attackOptions[k];
                        AIChoice newChoice = new AIChoice(tileChoice, faceChoice, attackChoice);
                        if (newChoice.Heuristic() > choice.Heuristic())
                        {
                            choice = newChoice;
                        }
                    }
                }
                else
                {
                    AIChoice newChoice = new AIChoice(tileChoice, faceChoice, null);
                    if (newChoice.Heuristic() > choice.Heuristic())
                    {
                        choice = newChoice;
                    }
                }
            }
        }
    }

    // run every turn to give the AI the info it needs
    public void CreateDataForAI()
    {
        tileOptions.Clear();
        List<Tile> tilesWithinRange = HexMap.GetMovementTiles(unit);

        for (int i = 0; i < tilesWithinRange.Count; i++)
        {
            // create tile option
            TileOption tileOption = new TileOption(tilesWithinRange[i]);
            // create tile heuristic
            tileOption.heuristic = new TileHeuristic(tilesWithinRange[i], unit, /*enemies*/null, /*objectives*/null);
            // create the facing options for that tile
            for (int j = 0; j < 6; j++)
            {
                // create face option
                FaceOption faceOption = new FaceOption(tilesWithinRange[i], j);
                // create the face heuristic
                faceOption.heuristic = new FaceHeuristic();
                // create the attacking options for that tile and facing
                List<Unit> unitsWithinRange = GetPlayerUnitsInRange(tilesWithinRange[i], j);
                for (int k = 0; k < unitsWithinRange.Count; k++)
                {
                    // create attack option
                    AttackOption attackOption = new AttackOption(tilesWithinRange[i], unit);
                    // create the attack heuristic
                    attackOption.heuristic = new AttackHeuristic(unit, unitsWithinRange[k], tilesWithinRange[i], j);
                    // add attack option to face option
                    faceOption.attackOptions.Add(attackOption);
                }
                // add face option to tile option
                tileOption.faceOptions.Add(faceOption);
            }
            // add tile option to list of tile options
            tileOptions.Add(tileOption);
        }
    }

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
        unit.MakeAttacking();
    }

    public override void SetAttack()
    {
        if (choice.attackChoice != null)
        {
            Timing.RunCoroutine(unit.PerformAttack(choice.attackChoice.chosenUnit));
            SelectionController.ShowTarget(choice.attackChoice.chosenUnit);
        }
        else
        {
            unit.MakeDone();
        }
    }

    public override void SetFacing()
    {
        unit.facing = choice.faceChoice.direction;
        unit.MakeChoosingAction();
    }

    public override void SetMovement()
    {
        if (choice.tileChoice.chosenTile != unit.currentTile)
        {
            unit.SetPath(unit.GetShortestPath(choice.tileChoice.chosenTile));
            unit.MakeMoving(null);
        }
        else
        {
            unit.MakeFacing();
        }
    }

    private List<Unit> GetPlayerUnitsInRange(Tile tile, int direction)
    {
        List<Unit> units = new List<Unit>();
        List<Tile> attackableTiles = HexMap.GetAttackTiles(tile, unit, direction);
        for (int i = 0; i < attackableTiles.Count; i++)
        {
            if (attackableTiles[i].currentUnit != null && attackableTiles[i].currentUnit.IsPlayerUnit())
            {
                units.Add(attackableTiles[i].currentUnit);
            }
        }
        return units;
    }
}
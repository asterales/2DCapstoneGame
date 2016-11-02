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

    // this is inefficient just making it fast for now
    // not sure if list of ignore is needed, only will be
    // if we make all AI decisions at the same time instead
    // of when they have to move
    public void MakeChoice(List<Tile> ignore)
    {
        //Debug.Log("Making Choice");
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
                AIChoice newChoice = null;

                if (tileOptions[i].faceOptions[j].attackOptions.Count > 0)
                {
                    //Debug.Log("FOUND ATTACK OPTION");
                    for (int k = 0; k < tileOptions[i].faceOptions[j].attackOptions.Count; k++)
                    {
                        attackChoice = tileOptions[i].faceOptions[j].attackOptions[k];
                        newChoice = new AIChoice(tileChoice, faceChoice, attackChoice);
                        //if (newChoice.attackChoice != null)
                        //{
                        //    Debug.Log("FOUND ATTACK OPTION");
                        //}
                        //Debug.Log("Evaluating Choice");
                        //if (choice == null) Debug.Log("CHOICE IS NULL");
                        //if (newChoice == null) Debug.Log("New CHOICE IS NULL");
                        if (choice.IsNull() || newChoice.Heuristic() > choice.Heuristic())
                        {
                            choice = newChoice;
                        }
                    }
                }
                else
                {
                    newChoice = new AIChoice(tileChoice, faceChoice, null);
                    if (choice.IsNull() || newChoice.Heuristic() > choice.Heuristic())
                    {
                        choice = newChoice;
                    }
                }

                // Write Heuristic for Debugging Purposes
            }

            //Debug.Log("Position: " + tileChoice.chosenTile.position.row + "," + tileChoice.chosenTile.position.col + " Heur: " + choice.Heuristic());
        }
        Debug.Log("HEURISTIC CHOICE :: " + choice.Heuristic());
        //if (choice.attackChoice == null)
        //{
        //    Debug.Log("AttackChoice Null");
        //}
    }

    // run every turn to give the AI the info it needs
    public void CreateDataForAI()
    {
        Debug.Log("Creating Data");
        tileOptions.Clear();
        List<Tile> tilesWithinRange = HexMap.GetMovementTiles(unit);

        for (int i = 0; i < tilesWithinRange.Count; i++)
        {
            if (tilesWithinRange[i].currentUnit != null && tilesWithinRange[i].currentUnit != unit)
            {
                continue;
            }
            // create tile option
            TileOption tileOption = new TileOption(tilesWithinRange[i]);
            // create tile heuristic
            tileOption.heuristic = new TileHeuristic(tilesWithinRange[i], unit, /*enemies*/playerUnits, /*objectives*/null);
            // create the facing options for that tile
            for (int j = 0; j < 6; j++)
            {
                // create face option
                FaceOption faceOption = new FaceOption(tilesWithinRange[i], j);
                // create the attacking options for that tile and facing
                List<Unit> unitsWithinRange = GetPlayerUnitsInRange(tilesWithinRange[i], j);
                // create the face heuristic
                faceOption.heuristic = new FaceHeuristic(unitsWithinRange, tilesWithinRange[i], unit, j);
                for (int k = 0; k < unitsWithinRange.Count; k++)
                {
                    //Debug.Log("FOUND UNIT WITHIN RANGE");
                    // create attack option
                    AttackOption attackOption = new AttackOption(tilesWithinRange[i], unitsWithinRange[k]);
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
        Debug.Log("Loading Data");
        for (int i=0;i<tileOptions.Count;i++)
        {
            tileOptions[i].LoadOptionData();
        }
    }

    // run every turn to evaluate the AI data
    public void EvaluateDataForAI()
    {
        Debug.Log("Evaluating Data");
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
        CreateDataForAI();
        LoadDataForAI();
        EvaluateDataForAI();
        MakeChoice(new List<Tile>());
        // DEBUG CODE //
        //if (choice == null) Debug.Log("CHOICE IS NULL");
        //if (choice.tileChoice == null) Debug.Log("TILE CHOICE IS NULL");
        //if (choice.tileChoice.chosenTile == null) Debug.Log("CHOSEN TILE IS NULL");
        ////////////////
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

    public override void Initialize()
    {
        tileOptions = new List<TileOption>();
        // TEST CODE //
        weightFunction = new AIWeights();
        weightFunction.initializeAttackEasy();
        ///////////////
    }

    private List<Unit> GetPlayerUnitsInRange(Tile tile, int direction)
    {
        List<Unit> units = new List<Unit>();
        List<Tile> attackableTiles = HexMap.GetAttackTiles(tile, unit, direction);
        //if (attackableTiles.Count > 0) Debug.Log("HAS ATTACKABLE TILES");

        for (int i = 0; i < attackableTiles.Count; i++)
        {
            //if (attackableTiles[i].currentUnit != null) Debug.Log("FOUND OTHER UNIT");
            //if (attackableTiles[i].currentUnit != null && attackableTiles[i].currentUnit.IsPlayerUnit()) Debug.Log("FOUND PLAYER UBIT");
            if (attackableTiles[i].currentUnit != null && attackableTiles[i].currentUnit.IsPlayerUnit())
            {
                //Debug.Log("IS PLAYER UNIT");
                units.Add(attackableTiles[i].currentUnit);
            }
        }
        return units;
    }
}
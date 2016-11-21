using UnityEngine;
using System.Collections.Generic;
using MovementEffects;

// base class for heuristic based AIs
// heuristic based AIs assign a weight to every possible tile that they can move to
// tile with most weight is the decided move

public class HeuristicAI : UnitAI
{
    public List<TileOption> tileOptions;
    public AIWeights weightFunction;
    public AIChoice choice;
    public Unit closestEnemy;
    public Objective closestObjective;
    private bool attackStarted;

    // not sure if list of ignore is needed, only will be
    // if we make all AI decisions at the same time instead
    // of when they have to move
    public void MakeChoice(List<Tile> ignore)
    {
        float maxHeuristic = 0.0f;
        for (int i = 0; i < tileOptions.Count; i++)
        {
            TileOption option = tileOptions[i];
            float currentHeuristic = option.weight + option.bestFaceOption.weight;
            if (option.bestFaceOption.bestAttackOption != null) currentHeuristic += option.bestFaceOption.bestAttackOption.weight;
            if (currentHeuristic > maxHeuristic) maxHeuristic = currentHeuristic;
        }

        List<AIChoice> choices = new List<AIChoice>();
        for (int i = 0; i < tileOptions.Count; i++)
        {
            TileOption option = tileOptions[i];
            float currentHeuristic = option.weight + option.bestFaceOption.weight;
            if (option.bestFaceOption.bestAttackOption != null) currentHeuristic += option.bestFaceOption.bestAttackOption.weight;
            if (currentHeuristic == maxHeuristic) choices.Add(new AIChoice(option, option.bestFaceOption, option.bestFaceOption.bestAttackOption));
        }

        System.Random rnd = new System.Random();
        choice = choices[rnd.Next(0, choices.Count)];

        Debug.Log("HEURISTIC CHOICE :: " + choice.Heuristic());
        Debug.Log("FACE CHOICE :: " + choice.faceChoice.weight);
        Debug.Log("NUMBER OF OPTIMAL CHOICES :: " + choices.Count);
        Debug.Log("NUMBER OF TILES :: " + tileOptions.Count);
        if (choice.faceChoice.heuristic.closestEnemyUnit == null) Debug.Log("NO CLOSEST ENEMY");
        if (!choice.tileChoice.heuristic.hasMoved) Debug.Log("REFRAINED FROM MOVING");
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
        attackStarted = false;
        unit.MakeAttacking();
    }

    public override void SetAttack()
    {
        if (!attackStarted)
        {
            if (choice.attackChoice != null)
            {
                Timing.RunCoroutine(unit.PerformAttack(choice.attackChoice.chosenUnit));
                SelectionController.instance.ShowTarget(choice.attackChoice.chosenUnit);
                attackStarted = true;
            }
            else
            {
                unit.MakeDone();
            }
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

    public override void Reset()
    {
        base.Reset();
        attackStarted = false;
    }


    public override void Initialize()
    {
        tileOptions = new List<TileOption>();
        // TEST CODE //
        weightFunction = new AIWeights();
        weightFunction.initializeAttackEasy();
        //weightFunction.initializeDefenseEasy();
        ///////////////
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
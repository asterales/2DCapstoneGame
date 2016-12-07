using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using MovementEffects;

public class NewAI : UnitAI{
    private bool attackStarted;
    private Move move;

    public override void Initialize()
    {
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
            if (move.target)
            {
                attackStarted = true;
                Timing.RunCoroutine(unit.PerformAttack(move.target));
                SelectionController.instance.ShowTarget(move.target);
            }
            else
            {
                unit.MakeDone();
            }
        }
    }

    public override void SetFacing()
    {
        unit.facing = move.facing;
        unit.MakeChoosingAction();
    }

    public override void SetMovement()
    {
        List<Move> possibleMoves = HexMap.GetPossibleMoves(unit);
        float maxscore = 0;
        move = possibleMoves[0];
        maxscore = move.score();
        foreach (Move m in possibleMoves)
        {
            if (m.score() > maxscore)
            {
                move = m;
                maxscore = m.score();
            }
        }
        unit.SetPath(unit.GetShortestPath(move.destination));
        unit.MakeMoving();
    }
}

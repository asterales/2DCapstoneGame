using System.Collections.Generic;

public class FaceOption : AIOption
{
    public Tile chosenTile;
    public List<AttackOption> attackOptions;
    public AttackOption bestAttackOption;
    public FaceHeuristic heuristic;
    public int direction;
    public float weight;

    public FaceOption()
    {
        chosenTile = null;
        direction = -1;
        attackOptions = new List<AttackOption>();
        heuristic = null;
        bestAttackOption = null;
    }

    public FaceOption(Tile tile, int dir)
    {
        chosenTile = tile;
        direction = dir;
        attackOptions = new List<AttackOption>();
        heuristic = null;
        bestAttackOption = null;
    }

    public override void LoadOptionData()
    {
        heuristic.EvaluateData();
        for (int i = 0; i < attackOptions.Count; i++)
        {
            attackOptions[i].heuristic.closestEnemyUnit = heuristic.closestEnemyUnit;
            attackOptions[i].heuristic.closestObjective = heuristic.closestObjective;
            attackOptions[i].LoadOptionData();
        }
    }

    public override void EvaluateOptionData(AIWeights weights)
    {
        weight = heuristic.CalculateHeuristic(weights);
        for (int i = 0; i < attackOptions.Count; i++)
        {
            attackOptions[i].EvaluateOptionData(weights);
            if (bestAttackOption == null || attackOptions[i].weight > bestAttackOption.weight)
            {
                bestAttackOption = attackOptions[i];
            }
        }
    }
}


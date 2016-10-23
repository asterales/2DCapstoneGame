public class AttackOption : AIOption
{
    public Tile chosenTile;
    public int chosenDirection;
    public Unit chosenUnit;
    public AttackHeuristic heuristic;
    public float weight;

    public AttackOption()
    {
        chosenTile = null;
        chosenDirection = -1;
        chosenUnit = null;
        heuristic = null;
        weight = 0.0f;
    }

    public AttackOption(Tile tile, Unit unit)
    {
        chosenTile = tile;
        chosenDirection = -1;
        chosenUnit = unit;
        heuristic = null;
        weight = 0.0f;
    }

    public override void LoadOptionData()
    {
        heuristic.EvaluateData();
    }

    public override void EvaluateOptionData(AIWeights weights)
    {
        weight = heuristic.CalculateHeuristic(weights);
    }
}
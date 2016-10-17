public class AttackOption : AIOption
{
    public Tile chosenTile;
    public int chosenDirection;
    public Unit chosenUnit;
    public AttackHeuristic heuristic;

    public AttackOption()
    {
        chosenTile = null;
        chosenDirection = -1;
        chosenUnit = null;
        heuristic = null;
    }

    public AttackOption(Tile tile, Unit unit)
    {
        chosenTile = tile;
        chosenDirection = -1;
        chosenUnit = unit;
        heuristic = null;
    }

    public override void LoadOptionData()
    {
        // to be implemented
    }

    public override void EvaluateOptionData()
    {
        // to be implemented
    }
}

// Ignore below information. Currently refactoring

/*public class AttackOption
{
    public Unit unit;
    public int direction;
    public int weight;

    public AttackOption()
    {
        unit = null;
        direction = -1;
        weight = -1000000;
    }

    public AttackOption(Unit u, int w, int dir)
    {
        unit = u;
        weight = w;
        direction = dir;
    }
}*/

public class UnitHeuristic {
    public Unit unit;
    public int weight;

    public UnitHeuristic()
    {
        unit = null;
        weight = -1000000;
    }

    public UnitHeuristic(Unit u, int w)
    {
        unit = u;
        weight = w;
    }
}

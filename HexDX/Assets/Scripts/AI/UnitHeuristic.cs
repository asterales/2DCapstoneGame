public class UnitHeuristic {
    public Unit unit;
    public int direction;
    public int weight;

    public UnitHeuristic()
    {
        unit = null;
        direction = -1;
        weight = -1000000;
    }

    public UnitHeuristic(Unit u, int w, int dir)
    {
        unit = u;
        weight = w;
        direction = dir;
    }
}

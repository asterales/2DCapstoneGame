// Ignore implementation for now. Currently in the process of refactoring

public class AttackOption
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
}

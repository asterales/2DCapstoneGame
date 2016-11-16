public class Capture : VictoryCondition
{
    public Tile goal;

    void Start()
    {
    }

    public override bool Achieved()
    {
        return goal.currentUnit != null && goal.currentUnit.IsPlayerUnit();
    }
}
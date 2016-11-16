public class KillCommander: VictoryCondition
{
    public Unit commander;

    void Start()
    {
    }

    public override bool Achieved()
    {
        return !commander.gameObject.activeSelf;
    }
}
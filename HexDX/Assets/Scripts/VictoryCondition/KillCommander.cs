public class KillCommander: VictoryCondition {
    public Unit commander;

    public override bool Achieved() {
        return !commander.gameObject.activeSelf; 
    }
}
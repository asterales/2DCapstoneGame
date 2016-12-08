public class Capture : VictoryCondition {
    public Tile goal;

    public override bool Achieved() {
        return goal.currentUnit != null && goal.currentUnit.IsPlayerUnit();
    }
}
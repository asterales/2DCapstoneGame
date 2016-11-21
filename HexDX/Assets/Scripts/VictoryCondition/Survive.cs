public class Survive : VictoryCondition {
    public int numTurns;

    public override void Init() {}

    public override bool Achieved() {
        return BattleController.instance.numTurns == numTurns;
    }
}
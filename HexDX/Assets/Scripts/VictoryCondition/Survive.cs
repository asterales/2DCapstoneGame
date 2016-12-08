public class Survive : VictoryCondition {
    public int numTurns;
    void Update()
    {
        victoryConditionText.text = "survive for "+(numTurns-BattleController.instance.numTurns) +" more turns!";
    }
    public override bool Achieved() {
        return BattleController.instance.numTurns == numTurns;
    }
}
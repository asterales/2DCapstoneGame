public class Survive : VictoryCondition
{
    public int numTurns;

    public override bool Achieved()
    {
        return BattleController.numTurns == numTurns;
    }
}
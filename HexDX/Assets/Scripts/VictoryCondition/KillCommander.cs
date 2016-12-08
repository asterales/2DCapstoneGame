public class KillCommander: VictoryCondition {
    public Unit commander;
    void Update()
    {
        victoryConditionText.text = "kill the commander!";
    }
    public override bool Achieved() {
        return !commander.gameObject.activeSelf; 
    }
}
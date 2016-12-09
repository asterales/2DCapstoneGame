public class Annihilation : VictoryCondition {
	public AIBattleController ai;
    void Update() {
        victoryConditionText.text = "rout the enemy!";
    }

	public override bool Achieved() {
		return ai.IsAnnihilated();
	}
}
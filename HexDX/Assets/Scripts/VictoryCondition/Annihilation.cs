public class Annihilation : VictoryCondition {
	public AIBattleController ai;
    void Update() {
        victoryConditionText.text = "route the enemy";
    }

	public override bool Achieved() {
		return ai.IsAnnihilated();
	}
}
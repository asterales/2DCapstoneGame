public class Annihilation : VictoryCondition {
	public AIBattleController ai;

	public override bool Achieved() {
		return ai.IsAnnihilated();
	}
}
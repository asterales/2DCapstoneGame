public class Annihilation : VictoryCondition {
	public AIBattleController ai;

	public override void Init() {
		ai = BattleManager.instance.ai;
	}

	public override bool Achieved() {
		return ai.IsAnnihilated();
	}
}
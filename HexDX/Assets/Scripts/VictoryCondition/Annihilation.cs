public class Annihilation : VictoryCondition {
	public AIBattleController ai;

	public override void Init() {
		ai = BattleControllerManager.instance.ai;
	}

	public override bool Achieved() {
		return ai.IsAnnihilated();
	}
}
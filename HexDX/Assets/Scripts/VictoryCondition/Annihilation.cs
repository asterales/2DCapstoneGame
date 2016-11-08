public class Annihilation : VictoryCondition {
	public AIBattleController ai;

	void Start() {
		ai = BattleControllerManager.instance.ai;
	}

	public override bool Achieved() {
		return ai.IsAnnihilated();
	}
}
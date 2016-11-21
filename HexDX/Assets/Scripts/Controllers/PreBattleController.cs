using UnityEngine;
using System.Collections;

public abstract class PreBattleController : MonoBehaviour {
	protected SelectionController sc;
	protected PlayerBattleController player;
	public bool isActive;

	protected virtual void Awake() {
		isActive = false;
	}

	private void Update() {
		if (isActive) {
			PhaseUpdateAction();
		}
	}

	protected abstract void PhaseUpdateAction();

	public virtual void StartPreBattlePhase() {
		Debug.Log("Starting phase for " + GetType());
		player = BattleControllerManager.instance.player;
		sc = SelectionController.instance;
		sc.ClearAllSelections();
		isActive = true;
	}

	public virtual void EndPreBattlePhase() {
		Debug.Log("Ending phase for " + GetType());
		sc.ClearAllSelections();
		isActive = false;
		enabled = false;
	}
}
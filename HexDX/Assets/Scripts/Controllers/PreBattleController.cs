using UnityEngine;
using System.Collections;

public abstract class PreBattleController : MonoBehaviour {
	protected PlayerBattleController player;
	public bool isActive;

	protected virtual void Awake() {
		isActive = false;
	}

	protected virtual void Start() {
		player = BattleControllerManager.instance.player;
	}

	private void Update() {
		if (isActive) {
			PhaseUpdateAction();
		}
	}

	protected abstract void PhaseUpdateAction();

	public virtual void StartPreBattlePhase() {
		Debug.Log("Starting phase for " + GetType());
		SelectionController.ClearAllSelections();
		isActive = true;
	}

	public virtual void EndPreBattlePhase() {
		Debug.Log("Ending phase for " + GetType());
		SelectionController.ClearAllSelections();
		isActive = false;
		enabled = false;
	}
}
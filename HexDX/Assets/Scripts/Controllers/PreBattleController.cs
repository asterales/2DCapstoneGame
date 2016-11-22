using UnityEngine;
using System.Collections;

public abstract class PreBattleController : MonoBehaviour {
	protected SelectionController sc;
	protected PlayerBattleController player;

	protected virtual void Awake() {
		enabled = false;
	}

	public virtual void StartPreBattlePhase() {
		Debug.Log("Starting phase for " + GetType());
		player = BattleControllerManager.instance.player;
		sc = SelectionController.instance;
		sc.ClearAllSelections();
		enabled = true;
	}

	public virtual void EndPreBattlePhase() {
		Debug.Log("Ending phase for " + GetType());
		sc.ClearAllSelections();
		enabled = false;
	}
}
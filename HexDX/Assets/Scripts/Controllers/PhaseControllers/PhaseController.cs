using UnityEngine;
using System.Collections;

public abstract class PhaseController : MonoBehaviour {
	protected SelectionController sc;
	private bool isActive;

	protected virtual void Awake() {
		enabled = false;
	}

	public virtual void StartBattlePhase() {
		Debug.Log("Starting phase for " + GetType());
		sc = SelectionController.instance;
		sc.ClearAllSelections();
		enabled = true;
	}

	public virtual void EndBattlePhase() {
		Debug.Log("Ending phase for " + GetType());
		SelectionController.instance.ClearAllSelections();
		enabled = false;
	}
}
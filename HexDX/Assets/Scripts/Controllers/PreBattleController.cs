using UnityEngine;
using System.Collections;

public abstract class PreBattleController : MonoBehaviour {
	public PlayerBattleController player;
    public AIBattleController ai;

    protected virtual void Start() {
    	DisableGameControllers();
    }

    protected void DisableGameControllers() {
		player.enabled = false;
		ai.enabled = false;
	}

	protected void EnableGameControllers() {
		player.enabled = true;
		ai.enabled = true;
	}

	public virtual void EndPreBattlePhase() {
		SelectionController.ClearAllSelections();
		EnableGameControllers();
		player.StartTurn();
		this.enabled = false;
	}
}
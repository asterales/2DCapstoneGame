using UnityEngine;
using MovementEffects;

public class ScriptedAttack : ScriptEvent {
    public Unit attacker;
    public Unit victim;

	protected override void Start () {
        base.Start();
	    ////// DEBUG CODE //////
        if (attacker == null)
        {
            Debug.Log("ERROR :: Attacker needs to be defined -> ScriptedAttack.cs");
        }
        if (victim == null)
        {
            Debug.Log("ERROR :: Victim needs to be defined -> ScriptedAttack.cs");
        }
        ////////////////////////
	}

    void Update() {
        if(isActive && isPlayerEvent){
            if (list.sc.target == victim && list.sc.target) {
                attacker.MakeAttacking();   
                Timing.RunCoroutine(attacker.PerformAttack(victim));
                list.sc.target = null;
                list.tutorial.targetTile = null;
            } else if (attacker.phase == UnitTurn.Done) {
                list.sc.mode = SelectionMode.ScriptedPlayerAttack;
                list.sc.selectedUnit = null;
                FinishEvent();
            }
        }
    }

    public override void DoPlayerEvent() {
        list.sc.mode = SelectionMode.ScriptedPlayerAttack;
        list.sc.selectedUnit = attacker;
        list.tutorial.targetTile = victim.currentTile;
    }

    public override void DoEvent() {
        list.sc.mode = SelectionMode.ScriptedAI;
        attacker.MakeAttacking();
        Timing.RunCoroutine(attacker.PerformAttack(victim));
        FinishEvent();
    }
}

using UnityEngine;
using System.Collections;

public class ScriptedAttack : ScriptEvent {
    public Unit attacker;
    public Unit victim;
    public int damageDelt;

	void Start () {
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
            if (SelectionController.target == victim) {
                float modifier = (float)damageDelt / (float)attacker.Attack;
                attacker.MakeAttacking();   
                StartCoroutine(attacker.DoAttack(victim, modifier));
                SelectionController.target = null;
                TutorialController.targetTile = null;
            } else if (attacker.phase == UnitTurn.Done) {
                SelectionController.mode = SelectionMode.ScriptedPlayerAttack;
                FinishEvent();
            }
        }
    }

    public override void DoPlayerEvent() {
        SelectionController.mode = SelectionMode.ScriptedPlayerAttack;
        TutorialController.targetTile = victim.currentTile;
    }

    public override void DoEvent() {
        SelectionController.mode = SelectionMode.ScriptedAI;
        attacker.MakeAttacking();
        StartCoroutine(attacker.PerformAttack(victim));
        FinishEvent();
    }
}

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class ScriptedChooseAction : ScriptEvent {
    public PlayerBattleController player;
    public Action action;
    public Unit unit;

    private bool[] actionEnabled = new bool[Enum.GetNames(typeof(Action)).Length];

    void OnGUI() {
        if (isActive && isPlayerEvent) {
            Vector3 pos = Camera.main.WorldToScreenPoint(unit.transform.position);

            if (player.GetSubmenuButton(pos, 1, "Attack", actionEnabled[(int)Action.Attack])) {
                FinishEvent();
            }
            if (player.GetSubmenuButton(pos, 2, "Wait", actionEnabled[(int)Action.Wait])) {
                Debug.Log("Waiting");
                SelectionController.selectedUnit = null;
                FinishEvent();
            }
            if (player.GetSubmenuButton(pos, 3, "Undo", actionEnabled[(int)Action.Undo])) {
                SelectionController.ResetLastTile(unit);
                FinishEvent();
            }
        }
    }

    public override void DoPlayerEvent() {
        player = BattleControllerManager.instance.player;
        SelectionController.mode = SelectionMode.ScriptedPlayerChooseAction;
        actionEnabled[(int)action] = true;
    }

    public override void DoEvent() {
        FinishEvent();
    }
}

public enum Action {
    Attack,
    Wait,
    Undo
}
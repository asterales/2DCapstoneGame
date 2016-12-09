using UnityEngine;
using System;

public class ScriptedChooseAction : ScriptEvent {
    public PlayerBattleController player;
    public Action action;
    public Unit unit;

    private bool[] actionEnabled = new bool[Enum.GetNames(typeof(Action)).Length];

    void OnGUI() {
        if (isPlayerEvent) {
            Vector3 pos = Camera.main.WorldToScreenPoint(unit.transform.position);

            if (player.GetSubmenuButton(pos, 1, "Attack", actionEnabled[(int)Action.Attack])) {
                GameManager.instance.PlayCursorSfx();
                FinishEvent();
            }
            if (player.GetSubmenuButton(pos, 2, "Wait", actionEnabled[(int)Action.Wait])) {
                Debug.Log("Waiting");
                GameManager.instance.PlayCursorSfx();
                list.sc.selectedUnit = null;
                FinishEvent();
            }
            if (player.GetSubmenuButton(pos, 3, "Undo", actionEnabled[(int)Action.Undo])) {
                GameManager.instance.PlayCursorSfx();
                list.sc.ResetLastTile(unit);
                FinishEvent();
            }
        }
    }

    protected override void EarlyCleanUp() { }

    public override void DoPlayerEvent() {
        player = BattleManager.instance.player;
        list.sc.mode = SelectionMode.ScriptedPlayerChooseAction;
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
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
            int itemHeight = 20;
            int itemWidth = 60;
            int offset = 60;
            Vector3 pos = CameraController.camera.WorldToScreenPoint(unit.transform.position);
            pos = new Vector3(pos.x, Screen.height - pos.y-offset);

            if (GUI.Button(new Rect(pos.x, pos.y, itemWidth, itemHeight), " Attack", player.GetGUIStyle(actionEnabled[(int)Action.Attack]))) {
                FinishEvent();
            }
            if (GUI.Button(new Rect(pos.x, pos.y+ itemHeight, itemWidth, itemHeight), " Wait", player.GetGUIStyle(actionEnabled[(int)Action.Wait]))) {
                Debug.Log("Waiting");
                SelectionController.selectedUnit = null;
                FinishEvent();
            }
            if (GUI.Button(new Rect(pos.x, pos.y + 2*itemHeight, itemWidth, itemHeight), " Undo", player.GetGUIStyle(actionEnabled[(int)Action.Undo]))) {
                SelectionController.ResetLastTile(unit);
                FinishEvent();
            }
        }
    }

    public override void DoPlayerEvent() {
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
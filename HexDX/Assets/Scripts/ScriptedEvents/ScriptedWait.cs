using UnityEngine;

public class ScriptedWait : ScriptEvent {
    public Unit unit;

	protected override void Start () {
        base.Start();
	    ////// DEBUG CODE //////
        if (unit == null) {
            Debug.Log("ERROR :: Unit needs to be defined");
        }
        ////////////////////////
	}

    public override void DoPlayerEvent() {
        list.sc.mode = SelectionMode.ScriptedPlayerWait;
        list.sc.selectedUnit = null;
        unit.MakeDone();
        FinishEvent();
    }

    public override void DoEvent() {
        list.sc.mode = SelectionMode.ScriptedAI;
        unit.MakeDone();
        FinishEvent();
    }
}

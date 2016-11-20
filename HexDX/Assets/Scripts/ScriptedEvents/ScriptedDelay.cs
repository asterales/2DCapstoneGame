using System.Collections.Generic;
using MovementEffects;

public class ScriptedDelay : ScriptEvent {
    public float seconds;

    public override void DoPlayerEvent() {
        Timing.RunCoroutine(Delay());
    }

    public override void DoEvent() {
        Timing.RunCoroutine(Delay());
    }

    public IEnumerator<float> Delay() {
        list.sc.mode = SelectionMode.ScriptedDelay;
        yield return Timing.WaitForSeconds(seconds);
        FinishEvent();
    }
}
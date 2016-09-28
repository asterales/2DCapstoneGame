using UnityEngine;
using System.Collections;
using System;

public class ScriptedDelay : ScriptEvent
{
    public float seconds;

    public override void DoPlayerEvent() {
        StartCoroutine(Delay());
    }

    public override void DoEvent() {
        Debug.Log("DOING EVENT");
        StartCoroutine(Delay());
    }

    public IEnumerator Delay()
    {
        SelectionController.mode = SelectionMode.ScriptedDelay;
        yield return new WaitForSeconds(seconds);
        FinishEvent();
    }
}
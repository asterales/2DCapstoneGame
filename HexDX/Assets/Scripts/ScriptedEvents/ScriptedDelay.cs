using UnityEngine;
using System.Collections;
using System;

public class ScriptedDelay : ScriptEvent
{
    public int seconds;
    private int counter;

    // I dont know how to use wait for seconds soooo, blah

    void Update()
    {
        if (counter > 0)
        {
            counter--;
            if (counter == 0)
            {
                FinishEvent();
            }
        }
    }

    public override void DoPlayerEvent()
    {
        counter = seconds * 60;
    }

    public override void DoEvent()
    {
        Debug.Log("DOING EVENT");
        counter = seconds * 60;
    }

    //public IEnumerable Delay()
    //{
    //    yield return new WaitForSeconds(3.0f);
    //}
}
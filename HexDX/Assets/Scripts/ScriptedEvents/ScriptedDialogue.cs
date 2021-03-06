﻿using UnityEngine;

/* 	Class for just scripted dialogue without action. 
	Dialogue currently handled by ScriptList class. 
	Set dialogue in instructions variable
*/

public class ScriptedDialogue : ScriptEvent {

	void Update() {
		if(Input.GetMouseButtonDown(0)){
			FinishEvent();
		}
	}

	public override void StartEvent() {
		base.StartEvent();
		list.dialogueMgr.hidePromptOnFinish = false;
	}

    public override void DoPlayerEvent() {  }

    public override void DoEvent() { }

    protected override void EarlyCleanUp() { }
}

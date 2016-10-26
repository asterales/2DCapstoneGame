using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

/* 	Class for just scripted dialogue without action. 
	Dialogue currently handled by ScriptList class. 
	Set dialogue in instructions variable

	For world map where certain buttons should be disabled
*/

public class ScriptedWorldMapDialogue : ScriptedDialogue {
	private static readonly List<string> buttonsToDisable = new List<string> {"Select", "Recruit" };
	private List<Button> disabledButtons;

    public override void DoPlayerEvent() {  
    	disabledButtons = buttonsToDisable.Select(s => GameObject.Find(s).GetComponent<Button>()).ToList();
    	disabledButtons.ForEach(b => b.enabled = false);
    }

    public override void FinishEvent() {
    	disabledButtons.ForEach(b => b.enabled = true);
    	base.FinishEvent();
    }

    public override void DoEvent() { }
}

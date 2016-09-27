using UnityEngine;
using System.Collections.Generic;

public class ScriptedAIBattleController : MonoBehaviour {
    public List<Unit> aiUnits;

	void Start () {
        ////// DEBUG CODE //////
	    if (aiUnits.Count == 0)
        {
            Debug.Log("ERROR :: Scripted AI has no units -> ScriptedAIBattleController.cs");
        }
        ///////////////////////
	}

    public void EndTurn()
    {
        for (int i = 0; i < aiUnits.Count; i++)
        {
            aiUnits[i].MakeOpen();
        }
    }

    public void StartTurn()
    {
        // may be needed
    }
}

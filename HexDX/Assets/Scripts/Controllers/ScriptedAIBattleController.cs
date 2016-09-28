using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

    public void HandOffAIControl(AIBattleController ai){
        ai.unitAIs = aiUnits.Where(u => u != null)
                            .Select(u => u.gameObject.GetComponent<UnitAI>())
                            .Where(a => a != null).ToList();
    }


    public void StartTurn()
    {
        // may be needed
    }
}

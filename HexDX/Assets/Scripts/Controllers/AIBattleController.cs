using UnityEngine;
using System.Collections.Generic;

public class AIBattleController : MonoBehaviour {
	//public bool FinishedTurn { get; set; }
	private List<Unit> units;

	void Start() {
		//FinishedTurn = false;
	}

    void Update() {
    	Debug.Log("AI Turn - not yet implemented");
    	//FinishedTurn = true;
    }

    public void StartTurn()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].phase = UnitTurn.Open;
        }
    }
}

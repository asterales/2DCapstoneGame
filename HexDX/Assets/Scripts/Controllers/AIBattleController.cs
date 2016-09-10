using UnityEngine;
using System.Collections.Generic;

public class AIBattleController : MonoBehaviour {
    //public bool FinishedTurn { get; set; }
    private List<Unit> units;

    //keeping track of last unit being modified last update
    private int currentUnitIndex;


    void Start() {
        //FinishedTurn = false;
    }

    void Update() {
        Debug.Log("AI Turn");
        if (SelectionController.TakingAIInput()) {
            MoveUnits();
        }
        //FinishedTurn = true;
    }

    public void StartTurn()
    {
        //for (int i = 0; i < units.Count; i++)
        //{
        //    units[i].phase = UnitTurn.Open;
        //}
        currentUnitIndex = 0;
        SelectionController.selectionMode = SelectionMode.AITurn;
    }
 
    // rename or change later
    private void MoveUnits() {
        if (currentUnitIndex < units.Count) {

        }
    }

    public void EndTurn()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].MakeOpen();
        }
    }
}

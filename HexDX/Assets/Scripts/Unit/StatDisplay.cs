using UnityEngine;
using System.Collections;

public class StatDisplay : MonoBehaviour {
    public static Unit selectedPlayerUnit;
    public static Unit selectedEnemyUnit;
    public static bool dirty;

	void Start () {
	
	}
	
	void Update () {
	    // update the display for the stats if they were changed
        if (dirty)
        {
            // to be implemented
        }
	}

    public static void DisplayPlayerUnit(Unit selectedUnit)
    {
        selectedPlayerUnit = selectedUnit;
        dirty = true;
    }

    public static void DisplayEnemyUnit(Unit selectedUnit)
    {
        selectedEnemyUnit = selectedUnit;
        dirty = true;
    }
}

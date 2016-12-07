using UnityEngine;

public class LEVictoryEditor : MonoBehaviour {
    public LEMapCache mapCache;
    public LEUnitCache unitCache;
    public LEVictoryTypeEditor victoryTypeEditor;
    public LESurvivalTurnEditor survivalTurnEditor;
    public LEKillSpecificUnitEditor killSpecificUnitEditor;

	void Awake () {
        ////// DEBUG CODE //////
        if (mapCache == null)
        {
            Debug.Log("ERROR :: Need Reference to the Map Cache -> LEMapCache.cs");
        }
	    if (unitCache == null)
        {
            Debug.Log("ERROR :: Need Reference to the Unit Cache -> LEVictoryEditor.cs");
        }
        if (survivalTurnEditor == null)
        {
            Debug.Log("ERROR :: Need Reference to the Survival Turn Editor -> LEVictoryEditor.cs");
        }
        if (victoryTypeEditor == null)
        {
            Debug.Log("ERROR :: Need Reference to the Victory Type Editor -> LEVictoryEditor.cs");
        }
        if (killSpecificUnitEditor == null)
        {
            Debug.Log("ERROR :: Need Reference to the KillSpecificUnitEditor -> LEVictoryEditor.cs");
        }
        ////////////////////////
	}

    public void TurnOn()
    {
        // to be implemented
    }

    public void TurnOff()
    {
        // to be implemented
    }
}

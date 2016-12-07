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
        TurnOff();
        ////////////////////////
	}

    public void TransitionToNext()
    {
        TurnOff();
        LEVictoryData data = mapCache.levels[mapCache.currentLevel].victoryData;
        if (data.routAllEnemies)
        {
            data.routAllEnemies = false;
            data.survival = true;
        }
        else if (data.survival)
        {
            data.survival = false;
            data.killSpecifiedUnit = true;
        }
        TurnOn();
    }

    public void TransitionToPrev()
    {
        TurnOff();
        LEVictoryData data = mapCache.levels[mapCache.currentLevel].victoryData;
        if (data.killSpecifiedUnit)
        {
            data.killSpecifiedUnit = false;
            data.survival = true;
        }
        else if (data.survival)
        {
            data.survival = false;
            data.routAllEnemies = true;
        }
        TurnOn();
    }

    public void TurnOn()
    {
        LEVictoryData data = mapCache.levels[mapCache.currentLevel].victoryData;
        victoryTypeEditor.TurnOn();
        if (data.routAllEnemies)
        {
            victoryTypeEditor.text.text = "Victory Type: Rout";
        }
        else if (data.survival)
        {
            victoryTypeEditor.text.text = "Victory Type: Survive";
            survivalTurnEditor.text.text = "Survive For: " + data.turnsToSurvive;
            survivalTurnEditor.TurnOn();
        }
        else if (data.killSpecifiedUnit)
        {
            victoryTypeEditor.text.text = "Victory Type: Assasin";
            killSpecificUnitEditor.text.text = "Kill Unit: " + data.indexOfDesiredUnit;
            killSpecificUnitEditor.TurnOn();
        }
    }

    public void TurnOff()
    {
        victoryTypeEditor.TurnOff();
        survivalTurnEditor.TurnOff();
        killSpecificUnitEditor.TurnOff();
    }
}

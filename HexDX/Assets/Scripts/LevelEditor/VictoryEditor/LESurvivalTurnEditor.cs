using UnityEngine;
using UnityEngine.UI;

public class LESurvivalTurnEditor : LEEditor {
    public LEMapCache mapCache;
    public LEUnitCache unitCache;
    public LEEditorIncrementButton incrementButton;
    public LEEditorIncrementButton decrementButton;
    public LEVictoryEditor reference;
    public Text text;

    void Start()
    {
        ////// DEBUG CODE //////
        if (mapCache == null)
        {
            Debug.Log("ERROR :: Need Reference to Map Cache -> LESurvivalTurnEditor.cs");
        }
        if (unitCache == null)
        {
            Debug.Log("ERROR :: Need Reference to Unit Cache -> LESurvivalTurnEditor.cs");
        }
        if (incrementButton == null)
        {
            Debug.Log("ERROR :: Need Reference to IncrementButton -> LESurvivalTurnEditor.cs");
        }
        if (decrementButton == null)
        {
            Debug.Log("ERROR :: Need Reference to DecrementButton -> LESurvivalTurnEditor.cs");
        }
        if (reference == null)
        {
            Debug.Log("ERROR :: Need Reference to VictoryEditor -> LESurvivalTurnEditor.cs");
        }
        if (text == null)
        {
            Debug.Log("ERROR :: Need Reference to Text -> LESurvivalTurnEditor.cs");
        }
        ////////////////////////
    }

    public override void TurnOn()
    {
        incrementButton.TurnOn();
        decrementButton.TurnOn();
        text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public override void TurnOff()
    {
        incrementButton.TurnOff();
        decrementButton.TurnOff();
        text.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    public override void Decrement(int modifier)
    {
        int num = mapCache.levels[mapCache.currentLevel].victoryData.turnsToSurvive;
        if (num > 0) mapCache.levels[mapCache.currentLevel].victoryData.turnsToSurvive--;
        reference.TurnOn(); // inefficient but i dont care
    }

    public override void Increment(int modifier)
    {
        mapCache.levels[mapCache.currentLevel].victoryData.turnsToSurvive++;
        reference.TurnOn(); // inefficient but i dont care
    }
}

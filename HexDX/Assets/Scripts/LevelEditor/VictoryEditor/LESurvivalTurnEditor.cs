using UnityEngine;
using UnityEngine.UI;

public class LESurvivalTurnEditor : LEEditor {
    public LEMapCache mapCache;
    public LEUnitCache unitCache;
    public LEEditorIncrementButton incrementButton;
    public LEEditorIncrementButton decrementButton;
    public Text text;

    void Start()
    {
        ////// DEBUG CODE //////
        if (mapCache == null)
        {
            Debug.Log("ERROR :: Need Reference to Map Cache -> LEKillSpecificUnitEditor.cs");
        }
        if (unitCache == null)
        {
            Debug.Log("ERROR :: Need Reference to Unit Cache -> LEKillSpecificUnitEditor.cs");
        }
        if (incrementButton == null)
        {
            Debug.Log("ERROR :: Need Reference to IncrementButton -> LEKillSpecificUnitEditor.cs");
        }
        if (decrementButton == null)
        {
            Debug.Log("ERROR :: Need Reference to DecrementButton -> LEKillSpecificUnitEditor.cs");
        }
        if (text == null)
        {
            Debug.Log("ERROR :: Need Reference to Text -> LEKillSpecificUnitEditor.cs");
        }
        ////////////////////////
    }

    public override void TurnOn()
    {
        incrementButton.TurnOn();
        decrementButton.TurnOn();
        // implement for text
    }

    public override void TurnOff()
    {
        incrementButton.TurnOff();
        decrementButton.TurnOff();
        // implement for text
    }

    public override void Decrement(int modifier)
    {
        // to be implemented
    }

    public override void Increment(int modifier)
    {
        // to be implemented
    }
}

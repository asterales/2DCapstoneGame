using UnityEngine;
using UnityEngine.UI;

public class LEVictoryTypeEditor : LEEditor {
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
            Debug.Log("ERROR :: Need Reference to Map Cache -> LEVictoryTypeEditor.cs");
        }
        if (unitCache == null)
        {
            Debug.Log("ERROR :: Need Reference to Unit Cache -> LEVictoryTypeEditor.cs");
        }
        if (incrementButton == null)
        {
            Debug.Log("ERROR :: Need Reference to IncrementButton -> LEVictoryTypeEditor.cs");
        }
        if (decrementButton == null)
        {
            Debug.Log("ERROR :: Need Reference to DecrementButton -> LEVictoryTypeEditor.cs");
        }
        if (reference == null)
        {
            Debug.Log("ERROR :: Need Reference to LEVictoryEditor -> LEVictoryTypeEditor.cs");
        }
        if (text == null)
        {
            Debug.Log("ERROR :: Need Reference to Text -> LEVictoryTypeEditor.cs");
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
        reference.TransitionToPrev();
    }

    public override void Increment(int modifier)
    {
        reference.TransitionToNext();
    }
}

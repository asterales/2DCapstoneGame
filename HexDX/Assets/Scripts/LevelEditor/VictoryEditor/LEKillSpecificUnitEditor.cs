using UnityEngine;
using UnityEngine.UI;

public class LEKillSpecificUnitEditor : LEEditor {
    public LEMapCache mapCache;
    public LEUnitCache unitCache;
    public LEEditorIncrementButton incrementButton;
    public LEEditorIncrementButton decrementButton;
    public LEVictoryEditor reference;
    public Text text;

	void Start () {
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
        if (reference == null)
        {
            Debug.Log("ERROR :: Need Reference to Victory Editor -> LEKillSpecificUnitEditor.cs");
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
        text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        if (mapCache.levels[mapCache.currentLevel].victoryData.indexOfDesiredUnit < unitCache.unitInstances.Count)
        {
            LEUnitInstance unit = unitCache.unitInstances[mapCache.levels[mapCache.currentLevel].victoryData.indexOfDesiredUnit];
            unit.spriteRenderer.color = Color.blue;
        }
    }

    public override void TurnOff()
    {
        incrementButton.TurnOff();
        decrementButton.TurnOff();
        text.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        if (mapCache.levels[mapCache.currentLevel].victoryData.indexOfDesiredUnit < unitCache.unitInstances.Count)
        {
            LEUnitInstance unit = unitCache.unitInstances[mapCache.levels[mapCache.currentLevel].victoryData.indexOfDesiredUnit];
            unit.spriteRenderer.color = Color.white;
        }
    }

    public override void Decrement(int modifier)
    {
        int num = mapCache.levels[mapCache.currentLevel].victoryData.indexOfDesiredUnit;
        num--;
        if (num > unitCache.unitInstances.Count - 1) num = unitCache.unitInstances.Count - 1;
        if (num < 0) num = 0;
        mapCache.levels[mapCache.currentLevel].victoryData.indexOfDesiredUnit = num;
        unitCache.ResetSprites();
        reference.TurnOn(); // inefficient but i dont care
    }

    public override void Increment(int modifier)
    {
        int num = mapCache.levels[mapCache.currentLevel].victoryData.indexOfDesiredUnit;
        num++;
        if (num > unitCache.unitInstances.Count - 1) num = unitCache.unitInstances.Count - 1;
        if (num < 0) num = 0;
        mapCache.levels[mapCache.currentLevel].victoryData.indexOfDesiredUnit = num;
        unitCache.ResetSprites();
        reference.TurnOn(); // inefficient but i dont care
    }
}

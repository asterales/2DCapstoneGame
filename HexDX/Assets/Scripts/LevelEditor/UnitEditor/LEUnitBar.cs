using UnityEngine;
using System.Collections.Generic;

public class LEUnitBar : MonoBehaviour {
    public LEUnitCache unitCache;
    public List<LEUnitBarButton> barButtons;
    public List<LEUnitSettings> unitTypes;
    private int currentIndex;
    private int numberOfButtons;
    private int chosenIndex;

    void Start()
    {
        ////// DEBUG CODE //////
        if (barButtons.Count != 4)
        {
            Debug.Log("ERROR :: References to UnitBarButtons are not set -> LEUnitBar.cs");
        }
        if (unitCache == null)
        {
            Debug.Log("ERROR :: Reference to UnitCache is not set -> LEUnitBar.cs");
        }
        ////////////////////////
        currentIndex = 0;
        numberOfButtons = barButtons.Count;
        chosenIndex = -1;
        unitTypes = unitCache.unitSettings;
        unitCache.unitBar = this;
        UpdateButtonsForIndex(0);
    }

    public void UpdateButtonsForIndex(int index)
    {
        chosenIndex = index;
        UpdateButtonSprites();
    }

    public void UpdateButtonSprites()
    {
        for (int i = 0; i < barButtons.Count; i++)
        {
            if (i + currentIndex < unitTypes.Count)
            {
                barButtons[i].SetAvatar(unitTypes[i + currentIndex]);
            }
        }
    }

    public void MoveUp()
    {
        //Debug.Log(unitTypes.Count);
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateButtonSprites();
            //Debug.Log("Moving Up");
        }
    }

    public void MoveDown()
    {
        if (currentIndex + numberOfButtons != unitTypes.Count)
        {
            currentIndex++;
            UpdateButtonSprites();
            //Debug.Log("Moving Down");
        }
    }
}

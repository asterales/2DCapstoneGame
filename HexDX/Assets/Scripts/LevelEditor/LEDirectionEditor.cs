using UnityEngine;
using System.Collections.Generic;

public class LEDirectionEditor : MonoBehaviour {
    public List<LEDirectionButton> directionButtons;
    public LEUnitInstance currentInstance;

    void Start () {
	    ////// DEBUG CODE //////
        if (directionButtons == null || directionButtons.Count != 6)
        {
            Debug.Log("ERROR :: Direction List not initialized -> LEDirectionEditor.cs");
        }
        ////////////////////////
        for (int i=0;i<directionButtons.Count;i++)
        {
            directionButtons[i].parent = this;
        }
	}
	
    public void UpdateDirection(int direction)
    {
        currentInstance.direction = direction;
        currentInstance.UpdateSprite();
        for (int i=0;i<directionButtons.Count;i++)
        {
            if (directionButtons[i].direction == direction)
            {
                directionButtons[i].TurnOn();
            }
            else
            {
                directionButtons[i].TurnOff();
            }
        }
    }

    public void TurnOn(LEUnitInstance instance)
    {
        currentInstance = instance;
        for (int i=0;i<directionButtons.Count;i++)
        {
            directionButtons[i].Activate();
        }
    }

    public void TurnOff()
    {
        for (int i=0;i<directionButtons.Count;i++)
        {
            directionButtons[i].DeActivate();
        }
    }
}

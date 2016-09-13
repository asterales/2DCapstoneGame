using UnityEngine;
using System.Collections.Generic;

public class LEHorizonBar : MonoBehaviour {
    public List<LEHorizonBarButton> buttons;
    public LEHorizonArrow rightButton;
    public LEHorizonArrow leftButton;
    public LESpriteVariantCache spriteCache;
    public int currentIndex;
    
	void Start () {
	    ////// DEBUG CODE //////
        if (buttons == null || buttons.Count == 0)
        {
            Debug.Log("ERROR :: HorizonBar Buttons need to be defined -> LEHorizonBar.cs");
        }
        if (rightButton == null)
        {
            Debug.Log("ERROR :: RightButton needs to be defined -> LEHorizonBar.cs");
        }
        if (leftButton == null)
        {
            Debug.Log("ERROR :: LeftButton needs to be defined -> LEHorizonBar.cs");
        }
        ////////////////////////
        TurnOff();
	}

    public void MoveRight()
    {
        ////// DEBUG CODE //////
        if (spriteCache == null)
        {
            Debug.Log("Sprite Cache is Not defined -> LEHorizonBar.cs");
        }
        ////////////////////////
        if (currentIndex + buttons.Count != spriteCache.sprites.Count)
        {
            currentIndex++;
            UpdateButtonSprites();
        }
    }

    public void MoveLeft()
    {
        ////// DEBUG CODE //////
        if (spriteCache == null)
        {
            Debug.Log("Sprite Cache is Not defined -> LEHorizonBar.cs");
        }
        ////////////////////////
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateButtonSprites();
        }
    }

    public void UpdateButtonSprites()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (spriteCache.sprites.Count > i)
            {
                buttons[i].SetAvatar(spriteCache.sprites[i + currentIndex]);
            }
        }
    }

    public void TurnOff()
    {
        for (int i=0;i<buttons.Count;i++)
        {
            buttons[i].TurnOff();
        }
        rightButton.TurnOff();
        leftButton.TurnOff();
    }

    public void TurnOn()
    {
        UpdateButtonSprites();
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].TurnOn();
        }
        rightButton.TurnOn();
        leftButton.TurnOn();
    }
}

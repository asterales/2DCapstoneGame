using UnityEngine;
using System.Collections.Generic;

public class LEHorizonBar : MonoBehaviour {
    public List<LEHorizonBarButton> buttons;
    public LETileBarButton relativeParent;
    public LEHorizonArrow rightButton;
    public LEHorizonArrow leftButton;
    public LESpriteVariantCache spriteCache;
    public int currentIndex;
    private int onCounter;
    
	void Start () {
	    ////// DEBUG CODE //////
        if (buttons == null || buttons.Count == 0)
        {
            Debug.Log("ERROR :: HorizonBar Buttons need to be defined -> LEHorizonBar.cs");
        }
        if (relativeParent == null)
        {
            Debug.Log("ERROR :: Reference to tileBarButton needs to be defined -> LEHorizonBar.cs");
        }
        if (rightButton == null)
        {
            Debug.Log("ERROR :: RightButton needs to be defined -> LEHorizonBar.cs");
        }
        if (leftButton == null)
        {
            Debug.Log("ERROR :: LeftButton needs to be defined -> LEHorizonBar.cs");
        }
        if (relativeParent == null)
        {
            Debug.Log("ERROR :: Tile Bar Button Reference needs to be defined -> LEHorizonBar.cs");
        }
        ////////////////////////
        onCounter = 10;
        TurnOff();
	}

    void Update()
    {
        if (onCounter < 10) onCounter++;
        else if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("Right Mouse Down");
            TurnOff();
        }
    }

    public void OnMakeChoice()
    {
        TurnOff();
        relativeParent.UpdateButton();
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
                buttons[i].SetAvatar(spriteCache, i + currentIndex);
            }
        }
    }

    public void TurnOff()
    {
        onCounter = 10;
        for (int i=0;i<buttons.Count;i++)
        {
            buttons[i].TurnOff();
        }
        rightButton.TurnOff();
        leftButton.TurnOff();
    }

    public void TurnOn()
    {
        onCounter = 0;
        UpdateButtonSprites();
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].TurnOn();
        }
        rightButton.TurnOn();
        leftButton.TurnOn();
    }
}

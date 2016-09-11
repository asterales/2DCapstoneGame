using UnityEngine;
using System.Collections.Generic;

public class LEHorizonBar : MonoBehaviour {
    public List<LEHorizonBarButton> buttons;
    public LESpriteVariantCache spriteCache;
    public int currentIndex;
    
	void Start () {
	    ////// DEBUG CODE //////
        if (buttons == null || buttons.Count == 0)
        {
            Debug.Log("ERROR :: HorizonBar Buttons need to be defined -> LEHorizonBar.cs");
        }
        ////////////////////////
	}

    public void MoveRight()
    {
        if (currentIndex + buttons.Count != spriteCache.sprites.Count)
        {
            currentIndex++;
            UpdateButtonSprites();
        }
    }

    public void MoveLeft()
    {
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

    public void DeExtend()
    {
        // to be implemented
    }
}

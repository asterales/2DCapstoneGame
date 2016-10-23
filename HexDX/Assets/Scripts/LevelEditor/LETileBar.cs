using UnityEngine;
using System.Collections.Generic;

public class LETileBar : MonoBehaviour {
    public LESpriteCache tileCache;
    public List<LETileBarButton> barButtons;
    public List<LESpriteVariantCache> spriteVariants;
    private int currentIndex;
    private int numberOfButtons;
    private int chosenIndex;
    
	void Start () {
        ////// DEBUG CODE //////
        if (tileCache == null)
        {
            Debug.Log("TileBar needs a reference to the tile cache -> LETileBar.cs");
        }
        spriteVariants = tileCache.variantCaches;
        ////////////////////////
        numberOfButtons = barButtons.Count;
        UpdateButtonsForIndex(0);
	}

    public void UpdateButtonsForIndex(int index)
    {
        chosenIndex = index;
        UpdateButtonSprites();
    }

    public void MoveUp()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateButtonsForIndex(currentIndex);
        }
    }

    public void MoveDown()
    {
        if (currentIndex + numberOfButtons != spriteVariants.Count)
        {
            currentIndex++;
            UpdateButtonsForIndex(currentIndex);
        }
    }

    public void UpdateButtonSprites()
    {
        for (int i = 0; i < barButtons.Count; i++)
        {
            if (i + chosenIndex < spriteVariants.Count)
            {
                barButtons[i].SetAvatar(spriteVariants[i + chosenIndex]);
            }
        }
    }

    public void TurnOffAllHorizontalBars()
    {
        for (int i = 0; i < barButtons.Count; i++)
        {
            barButtons[i].horizonBar.TurnOff();
        }
    }
}

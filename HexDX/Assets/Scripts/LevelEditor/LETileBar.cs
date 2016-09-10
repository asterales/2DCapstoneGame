using UnityEngine;
using System.Collections.Generic;

public class LETileBar : MonoBehaviour {
    public List<LESpriteVariantCache> objs;
    public List<LETileBarButton> barButtons;
    private int currentIndex;
    private int numberOfButtons;
    private int chosenIndex;
    
	void Start () {
        // maybe need
        chosenIndex = 0;
	}

    public void MoveUp()
    {
        if (currentIndex + numberOfButtons != objs.Count)
        {
            currentIndex++;
            UpdateButtonSprites();
        }
    }

    public void MoveDown()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateButtonSprites();
        }
    }

    public void UpdateButtonSprites()
    {
        for (int i = 0; i < barButtons.Count; i++)
        {
            if (objs.Count > i)
            {
                barButtons[i].SetAvatar(objs[i + currentIndex]);
            }
        }
    }
}

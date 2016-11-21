using UnityEngine;
using UnityEngine.UI;

public class LELevelScroll : MonoBehaviour {
    public LEMapCache mapCache;
    public LELevelScrollButton leftButton;
    public LELevelScrollButton rightButton;
    public Text text;

    void Start () {
	    ////// DEBUG CODE //////
        if (mapCache == null)
        {
            Debug.Log("ERROR :: Need Reference to MapCache -> LELevelScroll.cs");
        }
        if (leftButton == null)
        {
            Debug.Log("ERROR :: Need Reference to Left Button -> LELevelScrollButton.cs");
        }
        if (rightButton == null)
        {
            Debug.Log("ERROR :: Need Reference to Right Button -> LELevelScrollButton.cs");
        }
        if (text == null)
        {
            Debug.Log("ERROR :: Need Reference to the Text -> LELevelScrollButton.cs");
        }
        ////////////////////////
        leftButton.reference = this;
        rightButton.reference = this;
        text.text = mapCache.levels[mapCache.currentLevel].id;
	}
	
	public void ScrollRight()
    {
        if (mapCache.currentLevel != mapCache.levels.Count - 1)
        {
            mapCache.TransitionTo(mapCache.currentLevel + 1);
        }
        text.text = mapCache.levels[mapCache.currentLevel].id;
    }

    public void ScrollLeft()
    {
        if (mapCache.currentLevel != 0)
        {
            mapCache.TransitionTo(mapCache.currentLevel - 1);
        }
        text.text = mapCache.levels[mapCache.currentLevel].id;
    }
}

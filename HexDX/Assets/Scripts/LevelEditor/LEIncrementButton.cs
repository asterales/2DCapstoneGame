using UnityEngine;
using System.Collections;

public class LEIncrementButton : MonoBehaviour {
    public LEStatEditor parent;
    public int modifier;

	void Awake () {
	    ////// DEBUG CODE //////
        if (parent == null)
        {
            Debug.Log("ERROR :: Reference to StatEditor needs to be defined -> LEIncrementButton.cs");
        }
        ////////////////////////
	}

    public void TurnOn()
    {
        // to be implemented
    }

    public void TurnOff()
    {
        // to be implemented
    }
}

using UnityEngine;
using System.Collections;

public class LESelectionController : MonoBehaviour {
    public GameObject buttonSelectObj;
    public LETileButton selectedTileButton;
    
	void Start () {
        selectedTileButton = null;
        ////// DEBUG CODE //////
        if (buttonSelectObj == null)
        {
            Debug.Log("Button Select Obj needs to be defined -> LESelectionController.cs");
        }
        ////////////////////////
	}
	
	void Update () {
	    if (selectedTileButton == null)
        {
            //buttonSelectObj.transform.position = new Vector3(-1000, -1000, 0.9f);
        }
        else
        {
            Vector3 temp = selectedTileButton.transform.position;
            //buttonSelectObj.transform.position = new Vector3(temp.x, temp.y, 0.9f);
        }
	}
}

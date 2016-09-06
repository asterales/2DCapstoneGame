using UnityEngine;
using System.Collections;

public class LETileButton : MonoBehaviour {
    private GameObject buttonBackgroundObj;
    private GameObject tileSelectedObj;
    private GameObject tileTypeObj;
    public Sprite buttonBackgroundImg;
    public Sprite tileSelectedImg;
    public Sprite tileTypeImg;

    void Start()
    {
        ////// DEBUG CODE //////
        if (buttonBackgroundImg == null)
        {
            Debug.Log("Button Background Image Needs to be defined -> LETileButton.cs");
        }
        if (tileSelectedImg == null)
        {
            Debug.Log("Tile Selected Image Needs to be Defined -> LETileButton.cs");
        }
        if (tileTypeImg == null)
        {
            Debug.Log("Tyle Type Image Needs to be Defined -> LETileButton.cs");
        }
        ////////////////////////
    }

    void OnMouseDown()
    {
        // to be implemented
    }
}

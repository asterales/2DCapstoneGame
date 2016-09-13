using UnityEngine;
using System.Collections;

// This class is currently deprecated but it is being kept for reference

public class LETileButton : MonoBehaviour {
    // these will be automatically generated later
    // but for now we are hard coding everything
    //private GameObject buttonBackgroundObj;
    //private GameObject tileSelectedObj;
    //private GameObject tileTypeObj;

    /*public GameObject tileSelectObj;
    // HACKY AF
    public LESelectionController reference;
    public LETileButton other;

    public Sprite buttonBackgroundImg;
    public Sprite tileSelectedImg;
    public Sprite tileTypeImg;
    public int tileType;

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
        Vector3 otherPos = other.tileSelectObj.transform.localPosition;
        Vector3 currentPos = tileSelectObj.transform.localPosition;
        other.tileSelectObj.transform.localPosition = new Vector3(otherPos.x, otherPos.y, 0.05f);
        tileSelectObj.transform.localPosition = new Vector3(currentPos.x, currentPos.y, -0.05f);

        reference.selectedTileButton = this;
    }*/
}

using UnityEngine;
using System.Collections;

// this class is a test to see if one object is overlayed ontop of another and
// the overlayed one is clicked, do they both get clicked or does only the top
// one accept the click?

// If only the top one gets clicked, the overlay checking logic is not needed

// answer : Checking overlay in selection is not needed

public class ClickCollisionTest : MonoBehaviour {
    public string message = "Top Clicked";

    void OnMouseDown()
    {
        Debug.Log(message);
    }
}

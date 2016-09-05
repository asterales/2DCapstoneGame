using UnityEngine;
using System.Collections;

public class LELoadButton : MonoBehaviour {
    public LEMapLoader mapLoader;

    void OnMouseDown()
    {
        mapLoader.LoadLevel();
    }
}

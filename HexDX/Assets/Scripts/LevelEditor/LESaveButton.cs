using UnityEngine;
using System.Collections;

public class LESaveButton : MonoBehaviour {
    public LEMapWriter mapWriter;

    void OnMouseDown()
    {
        mapWriter.WriteLevel();
    }
}

using UnityEngine;
using System.Collections.Generic;

public class CanvasView : MonoBehaviour {
    private List<Canvas> canvases; // set in ui
    private bool enabled;

    void Start()
    {
        foreach (Transform child in transform)
        {
            Canvas canvas = child.gameObject.GetComponent<Canvas>();
            if (canvas != null) canvases.Add(canvas);
        }
        enabled = true;
    }
	
	void Update () {
	    if (Input.GetKeyDown("h"))
        {
            if (enabled) DisableCanvas();
            else EnableCanvas();
            enabled = !enabled;
        }
	}

    private void EnableCanvas()
    {
        foreach (Canvas canvas in canvases)
        {
            canvas.enabled = true;
        }
    }

    private void DisableCanvas()
    {
        foreach (Canvas canvas in canvases)
        {
            canvas.enabled = false;
        }
    }
}

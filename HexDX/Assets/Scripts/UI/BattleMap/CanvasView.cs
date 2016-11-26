using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CanvasView : MonoBehaviour {
    public List<Canvas> canvases; // set in ui
    private bool enableHUD;

    void Start() {
        foreach (Transform child in transform) {
            Canvas canvas = child.gameObject.GetComponent<Canvas>();
            if (canvas != null) canvases.Add(canvas);
        }
        enableHUD = true;
    }
	
	void Update () {
	    if (Input.GetKeyDown(KeyBindings.HIDE_HUD)) {
            if (enableHUD) DisableCanvas();
            else EnableCanvas();
            enableHUD = !enableHUD;
        }
	}

    private void EnableCanvas() {
        canvases.ForEach(c => c.enabled = true);
    }

    private void DisableCanvas() {
        canvases.ForEach(c => c.enabled = false);
    }
}

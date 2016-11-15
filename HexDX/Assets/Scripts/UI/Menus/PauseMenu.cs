using UnityEngine;

public class PauseMenu : MonoBehaviour {
    private bool paused = false;
    private SelectionMode lastMode;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (paused) {
                Unpause();
            } else {
                Pause();
            }
        }       
    }

    void OnGUI() {
        if (paused) {
            //Make a background box
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 250, 200), "Pause Menu");

            //Make Main Menu button
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 250, 50), "Main Menu")) {
                LevelManager.ReturnToMainMenu();
            }
            
            GUI.enabled = GameManager.instance && GameManager.instance.funds > 0; // prevent skip 1st tutorial 
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2, 250, 50), "Return To World Map")) {
                Unpause();
                LevelManager.ReturnToWorldMap();
            }
            GUI.enabled = true;

            //Make Change Graphics Quality button
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2+50, 250, 50), "Unpause")) {
                Unpause();
            }
        }
    }

    void Pause() {
        paused = true;
        GetComponent<CameraController>().enabled = false;
        Unit[] allUnits = FindObjectsOfType(typeof(Unit)) as Unit[];
        foreach (Unit unit in allUnits) {
            unit.enabled = false;
            unit.gameObject.GetComponent<Animator>().enabled = false;
            unit.gameObject.GetComponent<AudioSource>().enabled = false;
        }
        lastMode = SelectionController.mode;
        SelectionController.mode = SelectionMode.Paused;
    }

    void Unpause() {
        paused = false;
        GetComponent<CameraController>().enabled = true;
        Unit[] allUnits = FindObjectsOfType(typeof(Unit)) as Unit[];
        foreach (Unit unit in allUnits) {
            unit.enabled = true;
            unit.gameObject.GetComponent<Animator>().enabled = true;
            unit.gameObject.GetComponent<AudioSource>().enabled = true;
        }
        SelectionController.mode = lastMode;
    }
}
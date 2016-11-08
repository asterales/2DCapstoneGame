using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    bool paused = false;
    SelectionMode lastMode;

    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = true;
            GetComponent<CameraController>().enabled = false;
            Pause();
        }
         
    }

    void OnGUI()
    {
        if (paused)
        {
            //Make a background box
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 250, 200), "Pause Menu");

            //Make Main Menu button
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 250, 50), "Main Menu"))
            {
                LevelManager.ReturnToMainMenu();
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2, 250, 50), "Return To WorldMap"))
            {
                LevelManager.ReturnToWorldMap();
            }

            //Make Change Graphics Quality button
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2+50, 250, 50), "Unpause"))
            {
                GetComponent<CameraController>().enabled = true;
                paused = false;
                UnPause();
            }
        }
    }

    void Pause()
    {
        Unit[] allUnits = FindObjectsOfType(typeof(Unit)) as Unit[];
        foreach (Unit unit in allUnits)
        {
            unit.enabled = false;
            unit.gameObject.GetComponent<Animator>().enabled = false;
            unit.gameObject.GetComponent<AudioSource>().enabled = false;
        }
    }

    void UnPause()
    {
        Unit[] allUnits = FindObjectsOfType(typeof(Unit)) as Unit[];
        foreach (Unit unit in allUnits)
        {
            unit.enabled = true;
            unit.gameObject.GetComponent<Animator>().enabled = true;
            unit.gameObject.GetComponent<AudioSource>().enabled = true;
        }
    }
}
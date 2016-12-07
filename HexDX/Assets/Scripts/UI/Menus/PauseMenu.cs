using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    public Texture2D mainMenu;
    public Texture2D worldMap;
    public Texture2D unpause;
    public Texture2D mainMenuD;
    public Texture2D worldMapD;
    public Texture2D unpauseD;
    public Image background;
    private float width;
    private float height;
    private bool paused = false;
    private SelectionMode lastMode;

    void Start()
    {
        width = Screen.width * 0.35f;
        height = (width * mainMenu.height) / mainMenu.width;
    }
    void Update() {
        width = Screen.width * 0.35f;
        height = (width * mainMenu.height) / mainMenu.width;
        if (Input.GetKeyDown(KeyBindings.PAUSE)) {
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
            GUI.enabled = true;
            //Make Main Menu button
            if (GUI.Button(new Rect(Screen.width / 2 - width/2, Screen.height/5, width, height), "", GetGUIStyle(true, mainMenuD, mainMenu ))) {
                LevelManager.ReturnToMainMenu();
            }
            
            GUI.enabled = GameManager.instance && GameManager.instance.HasPassedFirstLevel(); // prevent skip 1st tutorial 
            if (GUI.Button(new Rect(Screen.width / 2 - width/2, Screen.height / 5 + height, width, height),"", GetGUIStyle(GUI.enabled, worldMapD, worldMap))) {
                Unpause();
                BattleController bc = BattleController.instance;
                if (!bc.BattleIsDone) {
                    bc.player.RestoreInitialArmyState();
                }
                if (GameManager.instance) {
                    GameManager.instance.UpdateArmyAfterBattle();
                }
                LevelManager.ReturnToWorldMap();
            }
            GUI.enabled = true;

            //Make Change Graphics Quality button
            if (GUI.Button(new Rect(Screen.width / 2 - width/2, Screen.height / 5 + 2*height, width, height),"",GetGUIStyle(true, unpauseD, unpause))) {
                Unpause();
            }
        }
    }

    private GUIStyle GetGUIStyle(bool active, Texture2D nohover, Texture2D hover)
    {
        GUIStyle style = new GUIStyle();
        style.normal.background = nohover;
        if (active)
        {
            GUI.enabled = true;
            style.hover.background = hover;
        }
        else
        {
            GUI.enabled = false;
            style.hover.background = nohover;
        }
        return style;
    }

    void Pause() {
        paused = true;
        background.enabled = true;
        background.color = Color.white;
        GetComponent<CameraController>().enabled = false;
        Unit[] allUnits = FindObjectsOfType(typeof(Unit)) as Unit[];
        foreach (Unit unit in allUnits) {
            unit.enabled = false;
            unit.gameObject.GetComponent<Animator>().enabled = false;
            unit.gameObject.GetComponent<AudioSource>().enabled = false;
        }
        lastMode = SelectionController.instance.mode;
        SelectionController.instance.mode = SelectionMode.Paused;
    }

    void Unpause() {
        paused = false;
        background.enabled = false;
        background.color = Color.clear;
        GetComponent<CameraController>().enabled = true;
        Unit[] allUnits = FindObjectsOfType(typeof(Unit)) as Unit[];
        foreach (Unit unit in allUnits) {
            unit.enabled = true;
            unit.gameObject.GetComponent<Animator>().enabled = true;
            unit.gameObject.GetComponent<AudioSource>().enabled = true;
        }
        SelectionController.instance.mode = lastMode;
    }
}
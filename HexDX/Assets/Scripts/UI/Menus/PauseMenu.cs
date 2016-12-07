using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    public Texture pauseMenu;
    public Texture mainMenu;
    public Texture worldMap;
    public Texture unpause;
    public Image background;
    private float width;
    private float height;
    private bool paused = false;
    private SelectionMode lastMode;

    void Start()
    {
        width = Screen.width * 0.35f;
        height = (width * pauseMenu.height) / pauseMenu.width;
    }
    void Update() {
        width = Screen.width * 0.35f;
        height = (width * pauseMenu.height) / pauseMenu.width;
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
            GUI.backgroundColor = Color.clear;

            GUI.enabled = true;
            //Make Main Menu button
            if (GUI.Button(new Rect(Screen.width / 2 - width/2, Screen.height/5, width, height), mainMenu)) {
                LevelManager.ReturnToMainMenu();
            }
            
            GUI.enabled = GameManager.instance && GameManager.instance.HasPassedFirstLevel(); // prevent skip 1st tutorial 
            if (GUI.Button(new Rect(Screen.width / 2 - width/2, Screen.height / 5 + height*0.9f, width, height), worldMap)) {
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
            if (GUI.Button(new Rect(Screen.width / 2 - width/2, Screen.height / 5 + 2*height*0.9f, width, height), unpause)) {
                Unpause();
            }
        }
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
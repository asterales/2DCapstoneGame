using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using MovementEffects;
using System.Collections;

/* 	Loads scenes that belongs to a level.
	Usage: attach to empty object in scene that can persist across scenes of the same level 
*/

public class LevelManager : MonoBehaviour {
    private static readonly string mainMenuSceneName = "MainMenu";
	private static readonly string battleSceneName = "TestScene";
	private static readonly string worldMapSceneName = "WorldMap";

    //public bool hasTutorial;
    //public bool doneTutorial;

	public static LevelManager activeInstance;

	// Level scene management
	public int levelId;
	public string mapFileName;
	public int moneyRewarded;
	
	private bool levelStarted;
	private bool returnedToWorldMap;

	private FadeTransition sceneFade;

	void Awake() {
		SceneManager.sceneLoaded += OnSceneLoaded;
		sceneFade = GetComponent<FadeTransition>();
	}

	void Start() {
		HideTutorialObjects();
		returnedToWorldMap = false;
		levelStarted = false;
	}

	private void HideTutorialObjects() {
		Transform tutorialTransform = transform.Find("TutorialObjects");
		if (tutorialTransform) {
			tutorialTransform.gameObject.transform.position = GameResources.hidingPosition;
		}
	}

	void OnDestroy() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	// delegate/event to be called when new scene is loaded
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		sceneFade.BeginFade(FadeDirection.In);
		if (returnedToWorldMap) {
			Destroy(gameObject);
		}
	}

	public void RegisterVictory(bool victory) {
		if (activeInstance != null && victory) {
			GameManager gm = GameManager.instance;
			if (!gm.defeatedLevelIds.Contains(activeInstance.levelId)) {
				gm.defeatedLevelIds.Add(activeInstance.levelId);
			}
			gm.funds += activeInstance.moneyRewarded;
		}
	}

	public static void ReturnToWorldMap() {
		if (GameManager.instance) {
			GameManager.instance.UpdateArmyAfterBattle();
		}
		if (activeInstance != null) {
			//with fade effects
			activeInstance.returnedToWorldMap = true;
            Timing.RunCoroutine(activeInstance.LoadScene(worldMapSceneName));
        } else {
        	// no fade effect b/c no fade texture associate with an levelmanager object
			SceneManager.LoadScene(worldMapSceneName);
        } 
	}

    public static void ReturnToMainMenu()
    {
        if (activeInstance != null)
        {
            //with fade effects
            activeInstance.returnedToWorldMap = true;
            Timing.RunCoroutine(activeInstance.LoadScene(mainMenuSceneName));
        }
        else
        {
            // no fade effect b/c no fade texture associate with an levelmanager object
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }

	private IEnumerator<float> LoadScene(string sceneName) {
		yield return Timing.WaitForSeconds(sceneFade.BeginFade(FadeDirection.Out));
		SceneManager.LoadScene(sceneName);
		Debug.Log("Loaded scene");
	}

	// for binding to onclick() event trigger
	public void StartLevel() {
		SetActiveLevel();
		levelStarted = true;
		Timing.RunCoroutine(LoadScene(battleSceneName));
	}

	private void SetActiveLevel() {
		activeInstance = this;
		DontDestroyOnLoad(gameObject);
	}

	public void NextScene() {
		if (!levelStarted) {
			StartLevel();
		} else {
			// change later to load also cutscenes
			ReturnToWorldMap();
		}
	}
}

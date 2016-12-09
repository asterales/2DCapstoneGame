using UnityEngine;
using UnityEngine.SceneManagement;
using MovementEffects;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

/* 	Loads scenes that belongs to a level.
	Usage: attach to empty object in scene that can persist across scenes of the same level 
*/

public class LevelManager : MonoBehaviour {
    private static readonly string mainMenuSceneName = "MainMenu";
	private static readonly string battleSceneName = "BattleMap";
	private static readonly string worldMapSceneName = "WorldMap";
	private static readonly string cutsceneSceneName = "Cutscene";
	private static readonly List<string> penultimateCutscenes = new List<string> { "PortalAssembly" };
	private static readonly string selectionSoundFile = "Clang_of_Metal";
	
	public static LevelManager activeInstance;

	// Level scene management
	public int levelId;
	public int moneyRewarded;
	public string mapFileName;
	public List<string> introCutscenes;
	public List<string> outroCutscenes;

	private bool levelStarted;
	private bool destroyOnLoad;
	private int currentSceneIndex;
	private List<SceneInfo> scenes;

	private FadeTransition sceneFade;

	private struct SceneInfo {
		public string sceneName;
		public string fileName;

		public SceneInfo(string scene, string file) {
			sceneName = scene;
			fileName = file;
		}
	}

	void Awake() {
		SceneManager.sceneLoaded += OnSceneLoaded;
		sceneFade = GetComponent<FadeTransition>();
		InitSceneList();
	}

	private void InitSceneList() {
		scenes = new List<SceneInfo>();
		scenes.AddRange(introCutscenes.Select(s => new SceneInfo(cutsceneSceneName, s)));
		scenes.Add(new SceneInfo(battleSceneName, mapFileName));
		scenes.AddRange(outroCutscenes.Select(s => new SceneInfo(cutsceneSceneName, s)));
		scenes = scenes.Where(s => s.fileName != null && s.fileName.Length > 0).ToList();
	}

	void Start() {
		HideTutorialObjects();
		destroyOnLoad = false;
		levelStarted = false;
		GameManager gm = GameManager.instance;
		if (gm && gm.defeatedLevelIds.Count == GameManager.TOTAL_NUM_LEVELS - 2) {
			outroCutscenes.AddRange(penultimateCutscenes);
			scenes.AddRange(penultimateCutscenes.Select(s => new SceneInfo(cutsceneSceneName, s)));
		}
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
		if (destroyOnLoad) {
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
		if (activeInstance != null) {
			//with fade effects
			activeInstance.destroyOnLoad = true;
            Timing.RunCoroutine(activeInstance.LoadScene(worldMapSceneName));
        } else {
        	// no fade effect b/c no fade texture associate with an levelmanager object
			SceneManager.LoadScene(worldMapSceneName);
        } 
	}

    public static void ReturnToMainMenu() {
        if (activeInstance != null) {
            //with fade effects
            activeInstance.destroyOnLoad = true;
            GameManager.DestroyCurrentInstance(); // delete saved state to start new game
            Timing.RunCoroutine(activeInstance.LoadScene(mainMenuSceneName));
        } else {
            // no fade effect b/c no fade texture associate with an levelmanager object
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    private static void Credits() {
        Timing.RunCoroutine(activeInstance.LoadScene("Credits"));
    }

    private IEnumerator<float> LoadScene(string sceneName) {
		yield return Timing.WaitForSeconds(sceneFade.BeginFade(FadeDirection.Out));
		SceneManager.LoadScene(sceneName);
		Debug.Log("Loaded scene");
	}

	// for binding to onclick() event trigger
	public void StartLevel() {
		SetActiveLevel();
		if (scenes.Count > 0) {
			levelStarted = true;
			Timing.RunCoroutine(LoadScene(scenes[0].sceneName));
		}
	}

	private void SetActiveLevel() {
		activeInstance = this;
		DontDestroyOnLoad(gameObject);
	}

	public void NextScene() {
		if (!levelStarted) {
			StartLevel();
		} else {
			currentSceneIndex++;
			if (currentSceneIndex < scenes.Count) {
				Timing.RunCoroutine(LoadScene(scenes[currentSceneIndex].sceneName));
			} else if (GameManager.instance) {
				GameManager gm = GameManager.instance;
				if (gm.defeatedLevelIds.Count < GameManager.TOTAL_NUM_LEVELS) {
					ReturnToWorldMap();
				} else {
					Credits();
				}
			}
		}
	}

	public string GetCurrentSceneFile() {
		return scenes[currentSceneIndex].fileName;
	}
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using MovementEffects;
using System.Collections;

/* 	Loads scenes that belongs to a level.
	Usage: attach to empty object in scene that can persist across scenes of the same level 

	Fading transition reference: https://unity3d.com/learn/tutorials/topics/graphics/fading-between-scenes */

public class LevelManager : MonoBehaviour {
	private static readonly string battleSceneName = "TestScene";
	private static readonly string worldMapSceneName = "WorldMap";

	public static LevelManager activeInstance;

	// Level scene management
	public int levelId;
	public string mapFileName;
	public int moneyRewarded;
	public GameObject tutorialObjs; // for levels with tutorial stuff

	private bool levelStarted;
	private bool returnedToWorldMap;

	// For fading transition effect
	public Texture2D loadingGraphic;
	public float fadeSpeed = 0.8f;
	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private FadeDirection fadeDir = FadeDirection.In;

	private AudioSource fadeOutMusic;

	void Awake() {
		GetTutorialObjs();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void GetTutorialObjs() {
		Transform tutorialTransform = transform.Find("TutorialObjects");
		if (tutorialTransform) {
			tutorialObjs = tutorialTransform.gameObject;
			tutorialObjs.SetActive(false);
		}
	}

	void Start() {
		returnedToWorldMap = false;
		levelStarted = false;
	}

	void OnDestroy() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	// delegate/event to be called when new scene is loaded
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		BeginFade(FadeDirection.In);
		MapLoader mapLoader = FindObjectOfType(typeof(MapLoader)) as MapLoader;
		if (mapLoader && tutorialObjs) {			
			tutorialObjs.SetActive(true);
		} else if (returnedToWorldMap) {
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
			activeInstance.returnedToWorldMap = true;
            Timing.RunCoroutine(activeInstance.LoadScene(worldMapSceneName));
        } else {
        	// no fade effect b/c no fade texture associate with an levelmanager object
			SceneManager.LoadScene(worldMapSceneName);
        } 
	}

	private IEnumerator<float> LoadScene(string sceneName) {
		yield return Timing.WaitForSeconds(BeginFade(FadeDirection.Out));
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

	void Update() {
		if (fadeOutMusic != null && fadeDir == FadeDirection.Out) {
			fadeOutMusic.volume = Mathf.Clamp01(fadeOutMusic.volume - (int)fadeDir * 1.5f * fadeSpeed * Time.deltaTime);
		}
	}

	void OnGUI() {
		alpha = Mathf.Clamp01(alpha + ((int)fadeDir * fadeSpeed * Time.deltaTime));
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), loadingGraphic);
	}

	private float BeginFade(FadeDirection direction) {
		fadeDir = direction;
		if (fadeDir == FadeDirection.Out) {
			fadeOutMusic = FindObjectOfType(typeof(AudioSource)) as AudioSource;
		}
		return fadeSpeed;
	}

}

public enum FadeDirection {
	In = -1, 
	Out = 1
}
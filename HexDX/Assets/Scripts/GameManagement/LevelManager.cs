using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

/* 	Loads scenes that belongs to a level.
	Usage: attach to empty object in scene that can persist across scenes of the same level 

	Fading transition reference: https://unity3d.com/learn/tutorials/topics/graphics/fading-between-scenes */

public class LevelManager : MonoBehaviour {
	// Level scene management
	public List<string> sceneNames;
	private int currentScene;
	private bool levelStarted;
	private static LevelManager activeInstance;

	// For fading transition effect
	public Texture2D loadingGraphic;
	public float fadeSpeed = 0.8f;
	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private FadeDirection fadeDir = FadeDirection.In;

	// Load next scene of active level
	public static void LoadNextScene() {
        if (activeInstance != null) {
            activeInstance.NextScene();
        } else {
            Debug.Log("No active level loader set");
        } 
	}

	// for binding to onclick() event trigger
	public void StartLevel() {
		SetActiveSingleton();
		if (sceneNames.Count > 0) {
			levelStarted = true;
			currentScene = 0;
			StartCoroutine(LoadScene(sceneNames[currentScene]));
		}
	}

	private void SetActiveSingleton() {
		levelStarted = false;
		activeInstance = this;
		currentScene = 0;
		DontDestroyOnLoad(gameObject);
	}

	private void NextScene() {
		if (!levelStarted) {
			StartLevel();
		} else {
			currentScene++;
			if (currentScene < sceneNames.Count) {
				StartCoroutine(LoadScene(sceneNames[currentScene]));
			} else {
				CompleteLevel();
			}
		}
	}

	private void CompleteLevel() {
		Debug.Log("Level complete");
		// maybe load back to world map at end of level?
		//Destroy(gameObject);
	}

	void OnGUI() {
		alpha = Mathf.Clamp01(alpha + ((int)fadeDir * fadeSpeed * Time.deltaTime));
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), loadingGraphic);
	}

	private float BeginFade(FadeDirection direction) {
		fadeDir = direction;
		return fadeSpeed;
	}

	private IEnumerator LoadScene(string sceneName) {
		yield return new WaitForSeconds(BeginFade(FadeDirection.Out));
		SceneManager.LoadScene(sceneName);
		yield return new WaitForSeconds(BeginFade(FadeDirection.In));
	}
}

public enum FadeDirection {
	In = -1, 
	Out = 1
}
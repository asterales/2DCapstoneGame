using UnityEngine;

/* 	Fading transition reference: https://unity3d.com/learn/tutorials/topics/graphics/fading-between-scenes */

public class FadeTransition : MonoBehaviour {

	// For fading transition effect
	public Texture2D loadingGraphic;
	public float fadeSpeed = 0.8f;
	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private FadeDirection fadeDir = FadeDirection.In;

	private AudioSource fadeOutMusic;

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

	public float BeginFade(FadeDirection direction) {
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
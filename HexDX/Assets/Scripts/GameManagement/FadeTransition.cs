using UnityEngine;
using UnityEngine.UI;

/* 	Fading transition reference: https://unity3d.com/learn/tutorials/topics/graphics/fading-between-scenes */

public class FadeTransition : MonoBehaviour {

	// Fade either applied to existing image or will draw a texture
	public Graphic fadeGraphic;
	public Texture2D loadingGraphic;
	public bool fadeBGM;

	// Paramenters
	public float fadeSpeed = 0.8f;
	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private FadeDirection fadeDir = FadeDirection.In;

	private AudioSource fadeOutMusic;

	void Update() {
		if (fadeBGM && fadeOutMusic != null && fadeDir == FadeDirection.Out) {
			fadeOutMusic.volume = Mathf.Clamp01(fadeOutMusic.volume - (int)fadeDir * 1.5f * fadeSpeed * Time.deltaTime);
		}
	}

	void OnGUI() {
		alpha = Mathf.Clamp01(alpha + ((int)fadeDir * fadeSpeed * Time.deltaTime));
		if (loadingGraphic) {
			GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
			GUI.depth = drawDepth;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), loadingGraphic);
		} else if (fadeGraphic) {
			Color fadeGraphicColor = fadeGraphic.color;
			fadeGraphic.color = new Color(fadeGraphicColor.r, fadeGraphicColor.g, fadeGraphicColor.b, alpha);
		}
	}

	public float BeginFade(FadeDirection direction) {
		fadeDir = direction;
		if (fadeDir == FadeDirection.Out && fadeBGM) {
			MusicController mc = FindObjectOfType(typeof(MusicController)) as MusicController;
			if (mc) {
				fadeOutMusic = mc.audio;
			} else {
				fadeOutMusic = Camera.main.GetComponent<AudioSource>();
			}
		}
		return (float) 1.0 / fadeSpeed;
	}

}

public enum FadeDirection {
	In = -1, 
	Out = 1
}
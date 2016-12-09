using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Credits : MonoBehaviour {
    private const float BASE_PERCENT_SCREEN = 3.1f; // rough percentage screen height base text takes to fully cross the screen
    private const float PERCENT_SCREEN_PER_LINE = 0.0666f; // based on text size

    public Text text;
    public AudioSource audiosource;
    private float speed;

    void Awake() {
        speed = (BASE_PERCENT_SCREEN * Screen.height) / audiosource.clip.length ;
    }

	void Start () {
        if (GameManager.instance && GameManager.instance.deadUnitNames.Count > 0) {
            text.text += "\n\nin loving memory of:\n\n";
            foreach (string name in GameManager.instance.deadUnitNames) {
                text.text += name + "\n";
            }
            text.text += "\nwho did not make the perilous\njourney back home.\nthrough their noble sacrifice we shall carry on";
            speed = ((BASE_PERCENT_SCREEN + PERCENT_SCREEN_PER_LINE * (GameManager.instance.deadUnitNames.Count + 7)) * Screen.height) / audiosource.clip.length ;
        }
        text.text += "\n\n\n\n made at the UT Austin GAMMA program\n\n\n\n\n\n\nTHANK YOU";
    }
	
	void Update () {
        if (audiosource.isPlaying) {
            transform.position = transform.position + speed * Time.deltaTime * Vector3.up;
        } else {
            LevelManager.ReturnToMainMenu();
        }
	}
}

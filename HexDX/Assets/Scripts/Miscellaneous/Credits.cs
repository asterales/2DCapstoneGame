using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Credits : MonoBehaviour {
    private const float BASE_PERCENT_SCREEN = 3.5f; // rough percentage screen height base text takes to fully cross the screen
    private const float PERCENT_SCREEN_PER_LINE = 0.0666f; // based on text size

    public Text text;
    public AudioSource audiosource;
    private float speed;

	void Start () {
        text.text += "\n";
        if (GameManager.instance) {
            foreach (string name in GameManager.instance.deadUnitNames) {
                text.text += name + "\n";
            }
            speed = ((BASE_PERCENT_SCREEN + PERCENT_SCREEN_PER_LINE * GameManager.instance.deadUnitNames.Count) * Screen.height) / audiosource.clip.length ;
        }
        text.text += "\nwho did not make the perilous \njourney back home. \n through their noble sacrifice we shall carry on\n\n\n\n made at the UT Austin GAMMA program\n\n\n\n\n\n\nTHANK YOU";
    }
	
	void Update () {
        if (audiosource.isPlaying) {
            transform.position = transform.position + speed * Time.deltaTime * Vector3.up;
        } else {
            LevelManager.ReturnToMainMenu();
        }
	}
}

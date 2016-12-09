using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {
    public AudioSource audio;
    public AudioSource selectSfx;
    public AudioClip victory;
    public AudioClip failure;
    public AudioClip steampunk;
    public AudioClip medieval;

    private BattleController battleController;

	void Start () {
        battleController = BattleManager.instance.battleController;
        audio = GetComponent<AudioSource>();
        SetSteamPunk();
	}
	
    public void SetSteamPunk() {
        audio.clip = steampunk;
        audio.Play();
        audio.loop = true;
    }

    public void SetMedieval() {
        audio.clip = medieval;
        audio.Play();
        audio.loop = true;
    }

    public void SetVictory() {
        audio.clip = victory;
        audio.Play();
        audio.volume = 1.0f;
        audio.loop = false;
    }

    public void SetFailure() {
        audio.clip = failure;
        audio.Play();
        audio.volume = 1.0f;
        audio.loop = true;
    }

    // Update is called once per frame
    void Update () {
        if (battleController.BattleIsDone) {
            if (BattleController.instance.PlayerWon) {
                if (audio.clip != victory) {
                    audio.volume -= .02f;
                    if (audio.volume < .1f) {
                        SetVictory();
                    }
                }
            } else {
                if (audio.clip != failure) {
                    audio.volume -= .02f;
                    if (audio.volume < .1f) {
                        SetFailure();
                    }
                } 
            }
        } else {
            if (BattleController.instance.IsPlayerTurn) {
                if (audio.clip == medieval) {
                    audio.volume -= .02f;
                    if (audio.volume < .1f) {
                        SetSteamPunk();
                    }
                } else if (audio.clip == steampunk) {
                    if (audio.volume < 1.0f) {
                        audio.volume += .02f;
                    }
                }
            } else {
                if (audio.clip == steampunk) {
                    audio.volume -= .02f;
                    if (audio.volume < .1f) {
                        SetMedieval();
                    }
                } else if (audio.clip == medieval) {
                    if (audio.volume < 1.0f) {
                        audio.volume += .02f;
                    }
                }
            }
        }
    }
}

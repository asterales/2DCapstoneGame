using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {
    public AudioSource audio;
    public AudioClip victory;
    public AudioClip steampunk;
    public AudioClip medieval;
	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        setSteamPunk();
	}
	
    public void setSteamPunk()
    {
        audio.clip = steampunk;
        audio.Play();
        audio.loop = true;
    }

    public void setMedieval()
    {
        audio.clip = medieval;
        audio.Play();
        audio.loop = true;
    }

    public void setVictory()
    {
        audio.clip = victory;
        audio.Play();
        audio.volume = 1.0f;
        audio.loop = false;
    }
    // Update is called once per frame
    void Update () {
        if (BattleController.PlayerWon)
        {
            if (audio.clip != victory)
            {
                audio.volume -= .01f;
                if (audio.volume < .1f)
                    setVictory();
            }
        }
        else if (BattleController.IsPlayerTurn)
        {
            if (audio.clip == medieval)
            {
                audio.volume -= .01f;
                if (audio.volume < .1f)
                    setSteamPunk();
            }
            else if (audio.clip == steampunk)
            {
                if (audio.volume < 1.0f)
                    audio.volume += .01f;
            }
        }
        else
        {
            if (audio.clip == steampunk)
            {
                audio.volume -= .01f;
                if (audio.volume < .1f)
                    setMedieval();
            }
            else if (audio.clip == medieval)
            {
                if (audio.volume < 1.0f)
                    audio.volume += .01f;
            }
        }

    }
}

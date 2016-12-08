﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Credits : MonoBehaviour {
    public Text text;
    public AudioSource audiosource;
	// Use this for initialization
	void Start () {
        text.text += "\n";
        if (GameManager.instance)
            foreach (string name in GameManager.instance.deadUnitNames)
                text.text += name+"\n";
        text.text += "\nwho did not make the perilous \njourney back home. \n through their noble sacrifice we shall carry on\n\n\n\n made at the UT Austin GAMMA program\n\n\n\n\n\n\nTHANK YOU";

	}
	
	// Update is called once per frame
	void Update () {
        if (audiosource.isPlaying)
            transform.position = transform.position + 0.5f * Vector3.up;
        else
            LevelManager.ReturnToMainMenu();
	}
}

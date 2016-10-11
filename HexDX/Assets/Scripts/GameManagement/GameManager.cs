using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance; //singleton

	// public for debugging in editor
	public List<Unit> playerAllUnits;
	public List<Unit> activeUnits;
	public int funds; 

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(this);
	}
}
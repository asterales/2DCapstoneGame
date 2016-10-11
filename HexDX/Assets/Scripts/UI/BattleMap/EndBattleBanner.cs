using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EndBattleBanner : MonoBehaviour {
	public GameObject winBanner;
	public GameObject lossBanner;

	void Awake() {
		winBanner = transform.Find("WinBanner").gameObject;
		lossBanner = transform.Find("LossBanner").gameObject;
	}

	void Start() {
		Hide();
	}

	public void ShowWin() {
		Debug.Log("Player Wins!");
		winBanner.SetActive(true);
	}

	public void ShowLoss() {
		Debug.Log("Player lost!");
		lossBanner.SetActive(true);
	}

	private void Hide() {
		winBanner.SetActive(false);
		lossBanner.SetActive(false);
	}
}
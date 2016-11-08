using UnityEngine;
using UnityEngine.UI;

public class EndBattleBanner : MonoBehaviour {
	public GameObject winBanner;
	public GameObject lossBanner;
	public GameObject winningsBox;

	void Awake() {
		winBanner = transform.Find("WinBanner").gameObject;
		lossBanner = transform.Find("LossBanner").gameObject;
		winningsBox = transform.Find("WinningsBox").gameObject;
	}

	void Start() {
		Hide();
	}

	public void ShowWin() {
		Debug.Log("Player Wins!");
		LevelManager lm = LevelManager.activeInstance;
		if (lm) {
			winningsBox.GetComponentInChildren<Text>().text = "You won " + lm.moneyRewarded + " gold!";
		} else {
			winningsBox.GetComponentInChildren<Text>().text = "You won 1000 gold!";
		}
		winBanner.SetActive(true);
		winningsBox.SetActive(true);
	}

	public void ShowLoss() {
		Debug.Log("Player lost!");
		lossBanner.SetActive(true);
	}

	private void Hide() {
		winBanner.SetActive(false);
		lossBanner.SetActive(false);
		winningsBox.SetActive(false);
	}
}
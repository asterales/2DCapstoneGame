using UnityEngine;
using UnityEngine.UI;
using MovementEffects;
using System.Collections.Generic;
using System.Linq;

public class EndBattleBanner : MonoBehaviour {
	private GameObject winItems;
	private GameObject lossItems;
	private List<Graphic> lossGraphics;
	private GameObject winningsBox;

	public bool FinishedDisplay { get; private set; }

	void Awake() {
		winItems = transform.Find("WinItems").gameObject;
		lossItems = transform.Find("LossItems").gameObject;
		lossGraphics = lossItems.GetComponentsInChildren<Graphic>().ToList();
		winningsBox = winItems.transform.Find("WinningsBox").gameObject;
		FinishedDisplay = false;
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
		winItems.SetActive(true);
		FinishedDisplay = true;
	}

	public void ShowLoss() {
		Debug.Log("Player lost!");
		Timing.RunCoroutine(PlayLossAnimation());
	}

	private IEnumerator<float> PlayLossAnimation() {
		lossGraphics.ForEach(g => g.enabled = true);
		Image grayOverlay = lossItems.transform.Find("GrayOverlay").GetComponent<Image>();
		yield return Timing.WaitForSeconds(grayOverlay.GetComponent<FadeTransition>().BeginFade(FadeDirection.Out));
		Image lossBanner = lossItems.transform.Find("LossBanner").GetComponent<Image>();
		Text defeatText = lossBanner.transform.Find("Text").GetComponent<Text>();
		lossBanner.GetComponent<FadeTransition>().BeginFade(FadeDirection.Out);
		yield return Timing.WaitForSeconds(defeatText.GetComponent<FadeTransition>().BeginFade(FadeDirection.Out));
		Text continuePrompt = lossItems.transform.Find("ContinueText").GetComponent<Text>();
		yield return Timing.WaitForSeconds(continuePrompt.GetComponent<FadeTransition>().BeginFade(FadeDirection.Out));
		FinishedDisplay = true;
	}

	private void Hide() {
		winItems.SetActive(false);
		lossGraphics.ForEach(g => g.enabled = false);
	}
}
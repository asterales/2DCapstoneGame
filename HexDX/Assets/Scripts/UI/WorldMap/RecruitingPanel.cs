using UnityEngine;
using UnityEngine.UI;

public class RecruitingPanel : WorldMapPopupPanel {
	private Button recruitButton;
	private Text fundsText;

	protected override void Awake() {
		base.Awake();
		fundsText = transform.Find("Funds").GetComponent<Text>();
		InitRecruitButton();
	}

	private void InitRecruitButton() {
		recruitButton = transform.Find("Recruit Button").GetComponent<Button>();
		recruitButton.onClick.AddListener(RecruitUnit);
	}

	protected void RecruitUnit() {

	}

	protected bool CanPurchase() {
		return false;
	}

	protected virtual void Update() {
		recruitButton.interactable = CanPurchase();
		fundsText.text = "Current Funds: " + GameManager.instance.funds;
	}

}
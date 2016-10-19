using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RecruitingPanel : WorldMapPopupPanel {
	public List<RecruitListing> unitChoices;
	private UnitStatDisplay statDisplay;
	private Button recruitButton;
	private Text recruitButtonText;
	private Text fundsText;
	private Text descriptionText;
	private RecruitListing selectedListing;

	protected override void Awake() {
		base.Awake();
		statDisplay = transform.Find("Stats Panel").GetComponent<UnitStatDisplay>();
		unitChoices = GetComponentsInChildren<RecruitListing>().ToList();
		fundsText = transform.Find("Funds").GetComponent<Text>();
		descriptionText = transform.Find("Description").GetComponent<Text>();
		InitRecruitButton();
	}

	private void InitRecruitButton() {
		recruitButton = transform.Find("Recruit Button").GetComponent<Button>();
		recruitButton.onClick.AddListener(RecruitUnit);
		recruitButtonText = recruitButton.transform.Find("Text").GetComponent<Text>();
	}

	void Start() {
		RegisterClickCallbacks();
	}

	private void RegisterClickCallbacks() {
		foreach (RecruitListing listing in unitChoices) {
			listing.onClickCallback = SetSelectedListing;
		}
	}

	private void RecruitUnit() {
		if (CanPurchase()) {
			GameManager gm = GameManager.instance;
			gm.funds -= selectedListing.cost;
			GameObject newUnitObj = selectedListing.InstantiateRecruit();
			gm.AddNewPlayerUnit(newUnitObj.GetComponent<Unit>());
			SetSelectedListing(null);
		}
	}

	private bool CanPurchase() {
		return selectedListing != null ? (selectedListing.cost <= GameManager.instance.funds) : false;
	}

	private void SetSelectedListing(RecruitListing listing) {
		if (selectedListing == null || selectedListing.unitPanel.unit) {
			selectedListing = listing;
			if (selectedListing) {
				statDisplay.DisplayUnit(selectedListing.unitPanel.unit);
				descriptionText.text = selectedListing.unitPanel.unit.unitStats.className + "\nCost: " + selectedListing.cost;
				recruitButtonText.text = "Recruit (Cost: " + selectedListing.cost + ")";
			} else {
				statDisplay.ClearDisplay();
				descriptionText.text = "";
				recruitButtonText.text = "Recruit";
			}
		}
	}

	public override void Show() {
		base.Show();
		SetSelectedListing(null);
	}

	protected virtual void Update() {
		recruitButton.interactable = CanPurchase();
		fundsText.text = "Current Funds: " + GameManager.instance.funds;
	}

}
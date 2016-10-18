using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RecruitingPanel : WorldMapPopupPanel {
	public List<RecruitListing> unitChoices;
	private Button recruitButton;
	private Text recruitButtonText;
	private Text fundsText;
	private Text descriptionText;
	private RecruitListing selectedListing;

	protected override void Awake() {
		base.Awake();
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
		AddClickHandlersToChoices();
	}

	private void AddClickHandlersToChoices() {
		foreach (RecruitListing listing in unitChoices) {
			UnitDisplayClickHandler clickHandler = listing.gameObject.AddComponent<UnitDisplayClickHandler>();
			clickHandler.onSingleClickCallback = DisplayUnitInfo;
		}
	}

	private void DisplayUnitInfo(UnitDisplay unitPanel) {
		if (unitPanel.unit) {
			SetSelectedListing(unitPanel.GetComponent<RecruitListing>());
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
		selectedListing = listing;
		if (selectedListing) {
			descriptionText.text = listing.unitPanel.unit.GetComponent<UnitStats>().className;
			recruitButtonText.text = "Recruit (Cost: " + selectedListing.cost + ")";
		} else {
			descriptionText.text = "";
			recruitButtonText.text = "Recruit";
		}
	}

	protected virtual void Update() {
		recruitButton.interactable = CanPurchase();
		fundsText.text = "Current Funds: " + GameManager.instance.funds;
	}

}
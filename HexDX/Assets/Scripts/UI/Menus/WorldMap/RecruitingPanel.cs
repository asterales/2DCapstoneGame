using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RecruitingPanel : WorldMapPopupPanel {
	public List<RecruitListing> unitChoices;
	private UnitStatDisplay statDisplay;
	private UnitDisplay unitDisplay;
	public Button recruitButton;
	private Text fundsText;
	private Text descriptionText;
	private RecruitListing selectedListing;

	protected override void Awake() {
		base.Awake();
		statDisplay = transform.Find("Stats Panel").GetComponent<UnitStatDisplay>();
		unitChoices = GetComponentsInChildren<RecruitListing>().ToList();
		fundsText = transform.Find("Funds").GetComponent<Text>();
		descriptionText = transform.Find("Description").GetComponent<Text>();
		unitDisplay = transform.Find("Unit Display").GetComponentInChildren<UnitDisplay>();
		InitRecruitButton();
	}

	private void InitRecruitButton() {
		recruitButton = transform.Find("Recruit Button").GetComponent<Button>();
		recruitButton.onClick.AddListener(RecruitUnit);
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
		if (selectedListing == null || selectedListing.unitDisplay.unit) {
			selectedListing = listing;
			if (selectedListing) {
				Unit unit = selectedListing.unitDisplay.unit;
				unitDisplay.unit = unit;
				statDisplay.DisplayUnitStats(unit);
				descriptionText.text = GetDescriptionText(selectedListing);
			} else {
				unitDisplay.unit = null;
				statDisplay.ClearDisplay();
				descriptionText.text = "";
			}
		}
	}

	private string GetDescriptionText(RecruitListing listing) {
		if (listing && listing.unitDisplay.unit) {
			StringBuilder sb = new StringBuilder();
			sb.Append("<b>Cost</b>: " + listing.cost + " gold\n");
			sb.Append(listing.description);
			return sb.ToString();
		}
		return "";
	}

	public override void Show() {
		base.Show();
		SetSelectedListing(null);
	}

	protected virtual void Update() {
		recruitButton.interactable = CanPurchase();
		fundsText.text = GameManager.instance.funds + " gold";
	}

}
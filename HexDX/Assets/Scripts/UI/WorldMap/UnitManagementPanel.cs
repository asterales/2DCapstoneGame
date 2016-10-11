using UnityEngine;
using UnityEngine.UI;

public abstract class UnitManagementPanel : MonoBehaviour {
	private Button backButton;
	private Button purchaseButton;
	private Text fundsText;

	protected virtual void Awake() {
		fundsText = transform.Find("Funds").GetComponent<Text>();
		InitBackButton();
		InitPurchaseButton();
	}

	private void InitBackButton() {
		backButton = transform.Find("Return Button").GetComponent<Button>();
		backButton.onClick.AddListener(Hide);
	}

	private void InitPurchaseButton() {
		purchaseButton = transform.Find("Purchase Button").GetComponent<Button>();
		purchaseButton.onClick.AddListener(PurchaseAction);
	}

	protected abstract void PurchaseAction();
	protected abstract bool CanPurchase();

	protected virtual void Update() {
		purchaseButton.interactable = CanPurchase();
		fundsText.text = "Current Funds: " + GameManager.instance.funds;
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	public void Show() {
		gameObject.SetActive(true);
	}

}
using UnityEngine;
using UnityEngine.UI;

public abstract class UnitManagementPanel : MonoBehaviour {
	private Button backButton;
	private Button purchaseButton;

	protected virtual void Awake() {
		InitBackButton();
		InitPurchaseButton();
	}

	private void InitBackButton() {
		backButton = transform.Find("Return Button").gameObject.GetComponent<Button>();
		backButton.onClick.AddListener(Hide);
	}

	private void InitPurchaseButton() {
		purchaseButton = transform.Find("Purchase Button").gameObject.GetComponent<Button>();
		purchaseButton.onClick.AddListener(PurchaseAction);
	}

	protected abstract void PurchaseAction();
	protected abstract bool CanPurchase();

	protected virtual void Update() {
		purchaseButton.interactable = CanPurchase();
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	public void Show() {
		gameObject.SetActive(true);
	}

}
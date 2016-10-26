using UnityEngine;
using UnityEngine.UI;

public abstract class WorldMapPopupPanel : MonoBehaviour {
	private Button backButton;
	public bool isActive;

	protected virtual void Awake() {
		isActive = true;
		InitBackButton();
	}

	private void InitBackButton() {
		backButton = transform.Find("Return Button").GetComponent<Button>();
		backButton.onClick.AddListener(Hide);
	}

	public virtual void Hide() {
		isActive = false;
		gameObject.SetActive(false);
	}

	public virtual void Show() {
		isActive = true;
		gameObject.SetActive(true);
	}
}
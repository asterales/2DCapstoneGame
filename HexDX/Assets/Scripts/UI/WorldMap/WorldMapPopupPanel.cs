using UnityEngine;
using UnityEngine.UI;

public abstract class WorldMapPopupPanel : MonoBehaviour {
	private Button backButton;

	protected virtual void Awake() {
		InitBackButton();
	}

	private void InitBackButton() {
		backButton = transform.Find("Return Button").GetComponent<Button>();
		backButton.onClick.AddListener(Hide);
	}

	public virtual void Hide() {
		gameObject.SetActive(false);
	}

	public virtual void Show() {
		gameObject.SetActive(true);
	}
}
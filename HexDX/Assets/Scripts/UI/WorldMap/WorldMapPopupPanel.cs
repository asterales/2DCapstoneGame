using UnityEngine;
using UnityEngine.UI;

public abstract class WorldMapPopupPanel : MonoBehaviour {
	private Button backButton;
	private Image raycastBlocker;
	public bool isActive;

	protected virtual void Awake() {
		isActive = true;
		raycastBlocker = transform.parent.Find("Raycast Blocker").GetComponent<Image>(); // assumes all panels are children of same parent
	}

	protected virtual void Start() {
		InitBackButton();
	}

	private void InitBackButton() {
		backButton = transform.Find("Return Button").GetComponent<Button>();
		backButton.onClick.AddListener(Hide);
		backButton.onClick.AddListener(GameManager.instance.PlayCursorSfx);
	}

	public virtual void Hide() {
		isActive = false;
		raycastBlocker.enabled = false;
		gameObject.SetActive(false);
	}

	public virtual void Show() {
		isActive = true;
		raycastBlocker.enabled = true;
		gameObject.SetActive(true);
	}
}
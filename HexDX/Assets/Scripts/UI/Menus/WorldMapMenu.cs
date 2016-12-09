using UnityEngine;
using UnityEngine.UI;

public class WorldMapMenu : MonoBehaviour {
	private Button selectActiveArmyButton;
	private Button recruitButton;
	private Button quitButton;

	public UnitSelectionPanel selectionPanel;
	public RecruitingPanel recruitingPanel;
	public QuitConfirmationBox quitConfirmPanel;

	void Awake() {
		selectActiveArmyButton = transform.Find("Select").GetComponent<Button>();
		recruitButton = transform.Find("Recruit").GetComponent<Button>();
		quitButton = transform.Find("Quit").GetComponent<Button>();
		if (selectActiveArmyButton == null) {
			Debug.Log("Select Active Army Button not found - WorldMapMenu.cs");
		}
		if (recruitButton == null) {
			Debug.Log("Recruit Button not found - WorldMapMenu.cs");
		}
		if (quitButton == null) {
			Debug.Log("Quit Button not found - WorldMapMenu.cs");
		}
	}

	void Start() {
		InactivatePanels();
		RegisterListeners();
	}

	private void InactivatePanels() {
		selectionPanel.Hide();
		recruitingPanel.Hide();
		quitConfirmPanel.Hide();
	}

	private void RegisterListeners() {
		selectActiveArmyButton.onClick.AddListener(selectionPanel.Show);
		selectActiveArmyButton.onClick.AddListener(GameManager.instance.PlayCursorSfx);
		recruitButton.onClick.AddListener(recruitingPanel.Show);
		recruitButton.onClick.AddListener(GameManager.instance.PlayCursorSfx);
		quitButton.onClick.AddListener(quitConfirmPanel.Show);
		quitButton.onClick.AddListener(GameManager.instance.PlayCursorSfx);
	}
}
using UnityEngine;
using UnityEngine.UI;

public class WorldMapMenu : MonoBehaviour {
	private Button selectActiveArmyButton;
	private Button recruitButton;
	private Button quitButton;

	public UnitSelectionPanel selectionPanel;
	public RecruitingPanel recruitingPanel;

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
	}

	private void RegisterListeners() {
		selectActiveArmyButton.onClick.AddListener(selectionPanel.Show);
		recruitButton.onClick.AddListener(recruitingPanel.Show);
		quitButton.onClick.AddListener(Quit);
	}

	private void Quit() {
		Debug.Log("Registered Quit Button");
		Application.Quit();
	}
}
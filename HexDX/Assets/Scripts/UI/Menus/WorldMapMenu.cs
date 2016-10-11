using UnityEngine;
using UnityEngine.UI;

public class WorldMapMenu : MonoBehaviour {
	private Button trainButton;
	private Button promoteButton;
	private Button recruitButton;
	private Button quitButton;

	public TrainingPanel trainingPanel;
	public PromotionPanel promotionPanel;
	public RecruitingPanel recruitingPanel;

	void Awake() {
		trainButton = transform.Find("Train").GetComponent<Button>();
		promoteButton = transform.Find("Promote").GetComponent<Button>();
		recruitButton = transform.Find("Recruit").GetComponent<Button>();
		quitButton = transform.Find("Quit").GetComponent<Button>();
		if (trainButton == null) {
			Debug.Log("Train Button not found - WorldMapMenu.cs");
		}
		if (promoteButton == null) {
			Debug.Log("Promote Button not found - WorldMapMenu.cs");
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
		trainingPanel.Hide();
		promotionPanel.Hide();
		recruitingPanel.Hide();
	}

	private void RegisterListeners() {
		trainButton.onClick.AddListener(trainingPanel.Show);
		promoteButton.onClick.AddListener(promotionPanel.Show);
		recruitButton.onClick.AddListener(recruitingPanel.Show);
		quitButton.onClick.AddListener(Quit);
	}

	private void Quit() {
		Debug.Log("Registered Quit Button");
		Application.Quit();
	}
}
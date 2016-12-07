using UnityEngine;

public class TutorialInfo : MonoBehaviour {
	public int tutorialId;
	public Character tutorialAdvisor;
	public int advisorPortraitIndex;

    void Awake() {
		tutorialAdvisor = Character.characters[2];
		advisorPortraitIndex = 9;
	}

	public bool HasBeenCompleted() {
		return GameManager.instance ? GameManager.instance.completedTutorials.Contains(tutorialId) : false;
	}

	public void RegisterCompleted() {
		GameManager gm = GameManager.instance;
		if(gm && !gm.completedTutorials.Contains(tutorialId)) {
			gm.completedTutorials.Add(tutorialId);
		}
	}
}
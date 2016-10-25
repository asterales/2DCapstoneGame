using UnityEngine;

public class TutorialInfo : MonoBehaviour {
	public Character tutorialAdvisor;
	public int advisorPortraitIndex;
	
	public string tutorialFocus;

	void Awake() {
		tutorialAdvisor = Character.characters[2];
		advisorPortraitIndex = 7;
	}

	public bool HasBeenCompleted() {
		return GameManager.instance.completedTutorials.Contains(tutorialFocus);
	}

	public void RegisterCompleted() {
		GameManager gm = GameManager.instance;
		if(!gm.completedTutorials.Contains(tutorialFocus)) {
			gm.completedTutorials.Add(tutorialFocus);
		}
	}
}
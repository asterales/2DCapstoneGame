using UnityEngine;
using System.Collections;

// This class will be responsible for handling Game Loop States

public class BattleController : MonoBehaviour {
	private AIBattleController ai;
	private PlayerBattleController player;

	void Start () {
	  ai = this.gameObject.GetComponent<AIBattleController>();
		player = this.gameObject.GetComponent<PlayerBattleController>();
		////// DEBUG CODE //////
    if (ai == null)
		{
			Debug.Log("Error :: AI Battle Controller not defined -> BattleController.cs");
		}
		if (player == null)
		{
			Debug.Log("Error :: Player Battle Controller not defined -> BattleController.cs");
		}
		////////////////////////
	}

	// Update is called once per frame
	void Update () {

	}
}

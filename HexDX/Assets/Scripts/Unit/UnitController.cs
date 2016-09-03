using UnityEngine;
using System.Collections;

// this class represents a Unit and stores its data

public class UnitController : MonoBehaviour {
  private UnitStats unitStats;
	private UnitFacing facingBonus;
	private UnitMovementCache movementCache;
	private int type; // we may want to represent types by something else

	// Use this for initialization
	void Start () {
	  unitStats = this.gameObject.GetComponent<UnitStats>();
		facingBonus = this.gameObject.GetComponent<UnitFacing>();
		movementCache = this.gameObject.GetComponent<UnitMovementCache>();

		////// DEBUG CODE //////
    if (unitStats == null)
		{
      Debug.Log("Unit Needs Unit Stats to be defined -> UnitController.cs");
		}
		if (facingBonus == null)
		{
      Debug.Log("Unit Needs Unit Facing to be defined -> UnitController.cs");
		}
		if (movementCache == null)
		{
      Debug.Log("Unit Needs MovementCache to be defined -> UnitController.cs");
		}
		////////////////////////
	}
}

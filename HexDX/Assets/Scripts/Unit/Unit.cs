using UnityEngine;
using System.Collections.Generic;

// this class represents a Unit and stores its data

public class Unit : MonoBehaviour {
    public Tile currentTile;
    public UnitStats unitStats;
    public List<Tile> pathableTiles;
	private UnitFacing facingBonus;
	private UnitMovementCache movementCache;
	private int type; // we may want to represent types by something else
    public int unitTranslationSpeed = 10;

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

    void Update()
    {
        if (transform.position != currentTile.transform.position)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTile.transform.position, 0.2f);
            transform.position = transform.position + new Vector3(0, 0, currentTile.transform.position.z-0.0001f);
        }
    }

    public bool IsReachableTile(Tile destinationTile){
        return destinationTile != null && pathableTiles != null && destinationTile.pathable && pathableTiles.Contains(destinationTile);
    }

    public void Move(Tile destinationTile) {
        currentTile = destinationTile;
    }

    public void SetTile(Tile newTile){
        GameObject unitObj = this.gameObject;
        currentTile.currentUnit = null;
        newTile.currentUnit = this;
        currentTile = newTile;
        unitObj.transform.parent = newTile.transform;
        unitObj.transform.localPosition = new Vector3(0, 0, 0);
    }

}

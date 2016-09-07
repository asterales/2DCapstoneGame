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
    }

    public bool IsReachableTile(Tile destinationTile){
        return destinationTile != null && pathableTiles != null && destinationTile.pathable && pathableTiles.Contains(destinationTile);
    }

    public bool Move(Tile destinationTile) {
        if(pathableTiles != null && IsReachableTile(destinationTile)) {
            GameObject unitObj = this.gameObject;
            Vector3 distLeft = destinationTile.transform.position - unitObj.transform.position;
            if (distLeft.magnitude == 0){
                SetTile(destinationTile);
                return true;
            } else {
                //walk rows, then columns (to be changed)
                if(distLeft.x != 0) {
                    int direction = distLeft.x < 0 ? -1 : 1;
                    float xMvt = Mathf.Min(Mathf.Abs(distLeft.x), unitTranslationSpeed * Time.deltaTime );
                    unitObj.transform.position += new Vector3(direction * xMvt, 0, 0);
                } else if (distLeft.y != 0) {
                    int direction = distLeft.y < 0 ? -1 : 1;
                    float yMvt = Mathf.Min(Mathf.Abs(distLeft.y), unitTranslationSpeed * Time.deltaTime );
                    unitObj.transform.position += new Vector3(0, direction * yMvt, 0);
                }
            }
        }
        return false;
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

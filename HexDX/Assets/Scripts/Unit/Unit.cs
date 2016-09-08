using UnityEngine;
using System.Collections.Generic;

// this class represents a Unit and stores its data

public class Unit : MonoBehaviour {
    public Tile currentTile;
    public UnitStats unitStats;
    public Queue<Vector3> path;
    public int phase = 0;
	private UnitFacing facingBonus;
	private UnitMovementCache movementCache;
	private int type; // we may want to represent types by something else
    public int unitTranslationSpeed = 10;


	// Use this for initialization
	void Start () {
        unitStats = this.gameObject.GetComponent<UnitStats>();
		facingBonus = this.gameObject.GetComponent<UnitFacing>();
		movementCache = this.gameObject.GetComponent<UnitMovementCache>();
        path = new Queue<Vector3>();


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
        Move();
    }

    private void Move() {
        if (path.Count > 0)
        {
            Vector3 destination = path.Peek() + new Vector3(0, 0, 0.999f);
            if (transform.position != destination)
                transform.position = Vector3.MoveTowards(transform.position, destination, 0.2f);
            else
                path.Dequeue();
        }

    }

    public void SetTile(Tile newTile){
        GameObject unitObj = this.gameObject;
        currentTile.currentUnit = null;
        newTile.currentUnit = this;
        currentTile = newTile;
        unitObj.transform.parent = newTile.transform;
    }

}

using UnityEngine;
using System.Collections.Generic;

// this class represents a Unit and stores its data

public class Unit : MonoBehaviour {
    public Tile currentTile;
    public UnitStats unitStats;
    public Queue<Tile> path;
    public UnitTurn phase;
    private UnitFacing facingBonus;
    private UnitMovementCache movementCache;
    private int type; // we may want to represent types by something else
    private readonly float maxMovement = 0.2f;

    private SpriteRenderer spriteRenderer;


    // Use this for initialization
    void Start () {
        unitStats = this.gameObject.GetComponent<UnitStats>();
        facingBonus = this.gameObject.GetComponent<UnitFacing>();
        movementCache = this.gameObject.GetComponent<UnitMovementCache>();
        path = new Queue<Tile>();

        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

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
        if (spriteRenderer == null)
        {
            Debug.Log("Unit Needs SpriteRenderer to be defined -> UnitController.cs");
        }
        ////////////////////////

        MakeOpen();
    }

    void Update() {
        Move();
    }

    private void Move() {
        if (path.Count > 0) {
            Vector3 destination = path.Peek().transform.position;
            if (transform.position != destination) {
                transform.position = Vector3.MoveTowards(transform.position, destination, maxMovement);
            }
            else {
                if (path.Count == 1)
                {
                    SetTile(path.Dequeue());
                    MakeDone();
                }
                else
                    currentTile = path.Dequeue();
            }
        }
    }

    public void SetTile(Tile newTile) {
        GameObject unitObj = this.gameObject;
        currentTile.currentUnit = null;
        newTile.currentUnit = this;
        currentTile = newTile;
        unitObj.transform.parent = newTile.transform;
    }

    // Phase Change Methods //
    public void MakeOpen()
    {
        phase = UnitTurn.Open;
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
    }

    public void MakeChoosingAction()
    {

    }

    public void MakeAttacking()
    {

    }

    public void MakeFacing()
    {

    }

    public void MakeDone()
    {
        phase = UnitTurn.Done;
        spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
    }
    ///////////////////////////
}

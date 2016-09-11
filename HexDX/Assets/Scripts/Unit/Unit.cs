using UnityEngine;
using System.Collections.Generic;

// this class represents a Unit and stores its data

public class Unit : MonoBehaviour {
    public Tile currentTile;
    public UnitStats unitStats;
    public UnitSprites sprites;
    public List<Vector2> attackablePositions;

    private Queue<Tile> path;
    public UnitTurn phase;
    public bool isPlayerUnit;
    public int facing;
    private UnitFacing facingBonus;
    private UnitMovementCache movementCache;
    private int type; // we may want to represent types by something else
    private readonly float maxMovement = 0.2f; 

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Tile lastTile;

    // Use this for initialization
    void Start () {
        unitStats = this.gameObject.GetComponent<UnitStats>();
        facingBonus = this.gameObject.GetComponent<UnitFacing>();
        movementCache = this.gameObject.GetComponent<UnitMovementCache>();
        sprites = this.gameObject.GetComponent<UnitSprites>();
        path = new Queue<Tile>();
        facing = 0;

        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        animator = this.gameObject.GetComponent<Animator>();

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
        switch(phase){
            case UnitTurn.Moving:
                Move();
                break;
            case UnitTurn.Facing:
                Face();
                break;

        }

    }

    private void Move() {
        if (path.Count > 0) {
            // disable the players ability to select
            SelectionController.selectionMode = SelectionMode.Moving;
            Vector3 destination = path.Peek().transform.position;
            lastTile = path.Peek();
            
            if (transform.position != destination) {
                transform.position = Vector3.MoveTowards(transform.position, destination, maxMovement);
            } else {

                if (path.Count == 1)
                {
                    SetTile(path.Dequeue());
                    MakeFacing();
                }
                else
                {
                    path.Dequeue();
                    facing = HexMap.GetNeighbors(lastTile).IndexOf(path.Peek());
                    spriteRenderer.sprite = sprites.walking[facing];
                    animator.runtimeAnimatorController = sprites.walkingAnim[facing];
                }
            }
        }
    }

    private void Face()
    {
        int angle = Angle(new Vector2(1, 0),Input.mousePosition-CameraController.camera.WorldToScreenPoint(transform.position));
        if (angle < 18 || angle > 342)
            facing = 0;
        else if (angle < 91)
            facing = 1;
        else if (angle < 164)
            facing = 2;
        else if (angle < 198)
            facing = 3;
        else if (angle < 271)
            facing = 4;
        else
            facing = 5;
        spriteRenderer.sprite = sprites.idle[facing];
        animator.runtimeAnimatorController = sprites.idleAnim[facing];
        HexMap.ShowAttackTiles(currentTile);
    }

    private int Angle(Vector2 from, Vector2 to)
    {
        Vector2 diff = to - from;
        int output = (int)(Mathf.Atan2(diff.y, diff.x)* 57.2957795131f);
        if (output < 0)
            output += 360;
        return output;
    }

    public void SetTile(Tile newTile) {
        GameObject unitObj = this.gameObject;
        currentTile.currentUnit = null;
        newTile.currentUnit = this;
        currentTile = newTile;
        unitObj.transform.parent = newTile.transform;
    }

    public void SetPath(List<Tile> nextPath) {
        path = new Queue<Tile>(nextPath);
    }

    // Phase Change Methods //
    public void MakeOpen()
    {
        phase = UnitTurn.Open;
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
        spriteRenderer.sprite = sprites.idle[facing];
        animator.runtimeAnimatorController = sprites.idleAnim[facing];
    }

    // Phase Change Methods //
    public void MakeMoving()
    {
        phase = UnitTurn.Moving;
    }

    public void MakeChoosingAction()
    {

    }

    public void MakeAttacking()
    {
        phase = UnitTurn.Attacking;
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
        spriteRenderer.sprite = sprites.attack[facing];
        animator.runtimeAnimatorController = sprites.attackAnim[facing];
    }

    public void MakeFacing()
    {
        phase = UnitTurn.Facing;

    }

    public void MakeDone()
    {
        phase = UnitTurn.Done;
        HexMap.ClearAttackTiles();
        spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        // re-enable the players ability to select
        if (!SelectionController.TakingInput() && !SelectionController.TakingAIInput())
        {
            SelectionController.selectionMode = SelectionMode.Open;
        }
    }
    ///////////////////////////
    public bool CanPathThrough(Tile tile) {
        return tile != null && tile.pathable &&
                (!tile.currentUnit || isPlayerUnit == tile.currentUnit.isPlayerUnit);
    }

    public List<Tile> GetShortestPath(Tile dest) {
        int bound = Cost(dest, currentTile);
        List<Tile> shortestPath = new List<Tile>();
        while (true) {
            int t = Search(dest,currentTile, 0, bound, ref shortestPath);
            if (t == -1) {
                shortestPath.Add(dest);
                break;
            }
            if (t == int.MaxValue) {
                break;
            }
            bound = t;
            shortestPath = new List<Tile>();
        }
        return shortestPath;
    }

    private int Search(Tile node, Tile dest, int g, int bound, ref List<Tile> currentPath) {
        int f = g + Cost(node, dest);
        if (f > bound) {
            return f;
        }
        if (node == dest) {
            return -1;
        }
        int min = int.MaxValue;
        foreach (Tile neighbor in HexMap.GetNeighbors(node)) {
            if (CanPathThrough(neighbor)) {
                int t = Search(neighbor, dest, g + 1, bound, ref currentPath);
                if (t == -1) {
                    currentPath.Add(neighbor);
                    return -1;
                }
                if (t < min) {
                    min = t;
                }
            }
        }
        return min;
    }
        
    private int Cost(Tile a, Tile b) {
        return System.Math.Max(System.Math.Abs(a.position.row- b.position.row), System.Math.Abs(a.position.col - b.position.col))/2;
    }
}

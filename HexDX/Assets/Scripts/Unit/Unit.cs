using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
            lastTile = path.Peek();
            Vector3 destination = lastTile.transform.position;
            if (transform.position != destination) {
                transform.position = Vector3.MoveTowards(transform.position, destination, maxMovement);
            } else {
                if (path.Count == 1) {
                    SetTile(path.Dequeue());
                    MakeFacing();
                } else {
                    path.Dequeue();
                    facing = HexMap.GetNeighbors(lastTile).IndexOf(path.Peek());
                    spriteRenderer.sprite = sprites.walking[facing];
                    animator.runtimeAnimatorController = sprites.walkingAnim[facing];
                }
            }
        }
    }

    private void Face() {
        spriteRenderer.sprite = sprites.idle[facing];
        animator.runtimeAnimatorController = sprites.idleAnim[facing];
        HexMap.ShowAttackTiles(currentTile);
    }

    public void SetTile(Tile newTile) {
        transform.position = newTile.transform.position;
        transform.parent = newTile.transform;
        if (currentTile) {
            currentTile.currentUnit = null;
        }
        newTile.currentUnit = this;
        currentTile = newTile;
    }

    public void SetPath(List<Tile> nextPath) {
        int pathLimit = unitStats.mvtRange + 1;
        int numExtra = nextPath.Count - pathLimit;
        if (numExtra > 0) {
            nextPath.RemoveRange(pathLimit, numExtra);
        }
        path = new Queue<Tile>(nextPath);
    }

    public void SetFacing(Vector2 directionVec) {
        int angle = Angle(new Vector2(1, 0), directionVec);
        if (angle < 18 || angle > 342) {
            facing = 0;
        } else if (angle < 91) {
            facing = 1;
        } else if (angle < 164) {
            facing = 2;
        } else if (angle < 198) {
            facing = 3;
        } else if (angle < 271) {
            facing = 4;
        } else {
            facing = 5;
        }
    }

    private int Angle(Vector2 from, Vector2 to) {
        Vector2 diff = to - from;
        int output = (int)(Mathf.Atan2(diff.y, diff.x)* 57.2957795131f);
        if (output < 0)
            output += 360;
        return output;
    }

    // Phase Change Methods //
    public void MakeOpen() {
        phase = UnitTurn.Open;
        spriteRenderer.color = Color.white;
        spriteRenderer.sprite = sprites.idle[facing];
        animator.runtimeAnimatorController = sprites.idleAnim[facing];
    }

    public void MakeMoving() {
        phase = UnitTurn.Moving;
    }

    public void MakeChoosingAction() {
        phase = UnitTurn.ChoosingAction;

    }

    public void MakeAttacking() {
        phase = UnitTurn.Attacking;

    }

    public void MakeFacing() {
        phase = UnitTurn.Facing;

    }

    public void MakeDone() {
        phase = UnitTurn.Done;
        HexMap.ClearAttackTiles();
        spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        spriteRenderer.sprite = sprites.idle[facing];
        animator.runtimeAnimatorController = sprites.idleAnim[facing];
    }

    public IEnumerator PerformAttack(Unit target) {
        spriteRenderer.sprite = sprites.attack[facing];
        animator.runtimeAnimatorController = sprites.attackAnim[facing];
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length/animator.speed);

        target.unitStats.health -= unitStats.attack;
        if (target.HasInAttackRange(this)) {
            unitStats.health -= max((target.unitStats.attack * 2) / 3, 1);
        }
        Image health = target.transform.Find("HealthBar").GetComponent<Image>();
        health.fillAmount = (float)target.unitStats.health / (float)target.unitStats.maxHealth;
        health = transform.Find("HealthBar").GetComponent<Image>();
        health.fillAmount = Mathf.Max(0,(float)unitStats.health / (float)unitStats.maxHealth);
        MakeDone();
        if (unitStats.health <= 0) {
            Destroy(gameObject);
        }
        if (target.unitStats.health <= 0) {
            Destroy(target.gameObject);
        }
    }

    private int max(int a, int b) { return a > b ? a : b; }
    ///////////////////////////
    // Pathing Methods //
    public bool CanPathThrough(Tile tile) {
        return tile != null && tile.pathable && !tile.currentUnit; // Temporary
            //(!tile.currentUnit || isPlayerUnit == tile.currentUnit.isPlayerUnit);
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
            if (neighbor == dest || CanPathThrough(neighbor)) { // hack to allow bfs to terminate even if dest tile is not pathable
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
        return (System.Math.Abs(-b.position.row+a.position.row-b.position.col+a.position.col)+System.Math.Abs(a.position.row- b.position.row)+System.Math.Abs(a.position.col - b.position.col))/2;
    }

    public bool HasInAttackRange(Unit other){
        return HexMap.GetAttackTiles(currentTile).Contains(other.currentTile);
    }
}

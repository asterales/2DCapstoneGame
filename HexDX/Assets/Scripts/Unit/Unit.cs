using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// this class represents a Unit and stores its data

public class Unit : MonoBehaviour {
    public Tile currentTile;
    private UnitStats unitStats;
    public UnitSprites sprites;
    public List<Vector2> attackablePositions;

    private Queue<Tile> path;
    public UnitTurn phase;
    public int facing;
    private UnitFacing facingBonus;
    private UnitMovementCache movementCache;
    private int type; // we may want to represent types by something else
    private readonly float maxMovement = 0.2f; 

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Tile lastTile;

    // temporary storage for scripted stuff
    ScriptedMove scriptedMove;

    // Properties for shorthand access to stats, includes tile modifiers
    public int MvtRange { get { return unitStats.mvtRange + currentTile.tileStats.mvtModifier; } }
    public int MaxHealth { get { return unitStats.maxHealth; } }
    public int Health {
        get { return unitStats.health; }
        set { unitStats.health = value; }
    }
    public int Attack { get { return unitStats.attack + currentTile.tileStats.attackModifier; } }
    public int Defense { get { return unitStats.defense + currentTile.tileStats.defenseModifier; } }
    public int Power { get { return unitStats.power + currentTile.tileStats.powerModifier; } }
    public int Resistance { get { return unitStats.resistance + currentTile.tileStats.resistanceModifier; } }

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

        scriptedMove = null;

        ////// DEBUG CODE //////
        if (unitStats == null)
        {
            Debug.Log("Unit Needs Unit Stats to be defined -> Unit.cs");
        }
        if (facingBonus == null)
        {
            Debug.Log("Unit Needs Unit Facing to be defined -> Unit.cs");
        }
        if (movementCache == null)
        {
            Debug.Log("Unit Needs MovementCache to be defined -> Unit.cs");
        }
        if (spriteRenderer == null)
        {
            Debug.Log("Unit Needs SpriteRenderer to be defined -> Unit.cs");
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
            if (SelectionController.TakingInput()) {
                SelectionController.mode = SelectionMode.Moving;
            }
            lastTile = path.Peek();
            Vector3 destination = lastTile.transform.position;
            if (transform.position != destination) {
                transform.position = Vector3.MoveTowards(transform.position, destination, maxMovement);
            } else {
                if (path.Count == 1) {
                    SetTile(path.Dequeue());
                    // re-enable the players ability to select
                    if(SelectionController.mode == SelectionMode.Moving) {
                        SelectionController.mode = SelectionMode.Facing;
                    }
                    MakeFacing();
                    if (scriptedMove != null)
                    {
                        scriptedMove.FinishEvent();
                        scriptedMove = null;
                    }
                } else {
                    path.Dequeue();
                    facing = HexMap.GetNeighbors(lastTile).IndexOf(path.Peek());
                    SetFacingSprites();
                }
            }
        }
    }


    private void SetFacingSprites() {
        int face = (facing + 1)%6;
        switch (phase) {
            case UnitTurn.Moving:
                if (face < 3) {
                    spriteRenderer.flipX = false;
                    spriteRenderer.sprite = sprites.walking[(facing+3)%3];
                    animator.runtimeAnimatorController = sprites.walkingAnim[(facing+3)%3];
                } else {
                    spriteRenderer.flipX = true;
                    spriteRenderer.sprite = sprites.walking[(6 - facing) % 3];
                    animator.runtimeAnimatorController = sprites.walkingAnim[(6 - facing) % 3];
                }
                break;
            case UnitTurn.Attacking:
                if (face < 3) {
                    spriteRenderer.flipX = false;
                    spriteRenderer.sprite = sprites.attack[(facing + 3) % 3];
                    animator.runtimeAnimatorController = sprites.attackAnim[(facing + 3) % 3];
                } else {
                    spriteRenderer.flipX = true;
                    spriteRenderer.sprite = sprites.attack[(6 - facing) % 3];
                    animator.runtimeAnimatorController = sprites.attackAnim[(6 - facing) % 3];
                }
                break;
            default:
                if (face < 3) {
                    spriteRenderer.flipX = false;
                    spriteRenderer.sprite = sprites.idle[(facing + 3) % 3];
                    animator.runtimeAnimatorController = sprites.idleAnim[(facing + 3) % 3];
                } else {
                    spriteRenderer.flipX = true;
                    spriteRenderer.sprite = sprites.walking[(6-facing)%3];
                    animator.runtimeAnimatorController = sprites.idleAnim[(6 - facing) % 3];
                }
                break;
        }
    }

    private void Face() {
        SetFacingSprites();
        HexMap.ShowAttackTiles(this);
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
        int pathLimit = MvtRange + 1;
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
        SetFacingSprites();
    }

    public void MakeMoving(ScriptedMove move = null) {
        phase = UnitTurn.Moving;
        scriptedMove = move;
    }

    public void MakeChoosingAction(ScriptEvent scriptEvent = null) {
        phase = UnitTurn.ChoosingAction;

    }

    public void MakeAttacking() {
        phase = UnitTurn.Attacking;
    }

    public void MakeFacing() {
        phase = UnitTurn.Facing;
    }

    public void MakeDone() {
        if (gameObject)
        {
            phase = UnitTurn.Done;
            HexMap.ClearAttackTiles();
            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
            SetFacingSprites();
        }
    }

    public IEnumerator PerformAttack(Unit target)
    {

        StartCoroutine(DoAttack(target, 1.0f));
        //if (target && target.gameObject && target.HasInAttackRange(this))
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length / 5.0f);
        if (target && target.Health > 0 && target.HasInAttackRange(this))
        {
            spriteRenderer.color = Color.red;
            target.MakeAttacking();
            StartCoroutine(target.DoAttack(this, .66f));
        }

    }

    public IEnumerator DoAttack(Unit target, float modifier)
    {
        SetFacingSprites();
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length / 5.0f);

        target.Health -= (int)(Attack * modifier);
        GameObject damage = new GameObject();
        damage.AddComponent<DamageIndicator>().SetDamage((int)(-Attack * modifier));
        damage.transform.position = target.transform.position+new Vector3(-1f, 6f, 0f);
        Image healthBar = target.transform.Find("HealthBar").GetComponent<Image>(); // Find() is expensive
        healthBar.fillAmount = (float)target.Health / (float)target.MaxHealth;
        target.MakeOpen();
        if (target.Health <= 0)
        {
            Destroy(target.gameObject);
        }
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length / 5.0f);
        MakeDone();
    }

    private int max(int a, int b) { return a > b ? a : b; }
    ///////////////////////////
    // Pathing Methods //
    public bool CanPathThrough(Tile tile) {
        return tile != null && tile.pathable 
                && (!tile.currentUnit || IsPlayerUnit() == tile.currentUnit.IsPlayerUnit())
                && unitStats.mvtLevel >= tile.tileStats.mvtDifficulty;
    }

    public List<Tile> GetShortestPath(Tile dest) {
        int bound = HexMap.Cost(dest, currentTile);
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
        int f = g + HexMap.Cost(node, dest);
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

    public bool HasInAttackRange(Unit other){
        return HexMap.GetAttackTiles(this).Contains(other.currentTile);
    }

    public bool IsPlayerUnit() {
        return gameObject.GetComponent<UnitAI>() == null;
    }
}

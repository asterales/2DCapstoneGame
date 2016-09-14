﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// this class represents a Unit and stores its data

public class Unit : MonoBehaviour {
    public Tile currentTile;
    public UnitStats unitStats;
    public UnitSprites sprites;
    public List<Vector2> attackablePositions;
    public static Texture2D menuItem;
    public static Texture2D menuItemHovered;

    private Tile previousTile;

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
            if (SelectionController.TakingInput()) {
                 SelectionController.selectionMode = SelectionMode.Moving;
            }
            lastTile = path.Peek();
            Vector3 destination = lastTile.transform.position;
            
            if (transform.position != destination) {
                transform.position = Vector3.MoveTowards(transform.position, destination, maxMovement);
            } else {
                if (path.Count == 1) {
                    SetTile(path.Dequeue());
                    // re-enable the players ability to select
                    if (!SelectionController.TakingInput() && !SelectionController.TakingAIInput()) {
                        SelectionController.selectionMode = SelectionMode.Facing;
                    }
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
        int angle = Angle(new Vector2(1, 0),Input.mousePosition-CameraController.camera.WorldToScreenPoint(transform.position));
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
        spriteRenderer.sprite = sprites.idle[facing];
        animator.runtimeAnimatorController = sprites.idleAnim[facing];
        HexMap.ShowAttackTiles(currentTile);
    }

    void OnGUI()
    {
        if (phase == UnitTurn.ChoosingAction)
        {
            int itemHeight = 20;
            int itemWidth = 60;
            int offset = 60;
            Vector3 pos = CameraController.camera.WorldToScreenPoint(transform.position);
            pos = new Vector3(pos.x, Screen.height - pos.y-offset);
            bool canAttack = false;
            foreach (Tile t in HexMap.GetAttackTiles(currentTile))
            {
                if (t.currentUnit != null)
                {
                    canAttack = true;
                    break;
                }
            }
            if (GUI.Button(new Rect(pos.x, pos.y, itemWidth, itemHeight), " Attack", getGUIStyle(canAttack)))
            {
                
                MakeAttacking();
            }

            if (GUI.Button(new Rect(pos.x, pos.y+ itemHeight, itemWidth, itemHeight), " Wait", getGUIStyle(true)))
            {
                MakeDone();
            }

            if (GUI.Button(new Rect(pos.x, pos.y + 2*itemHeight, itemWidth, itemHeight), " Undo", getGUIStyle(true)))
            {
                SetTile(previousTile);
                HexMap.ClearAllTiles();
                MakeOpen();
                HexMap.ShowMovementTiles(currentTile, unitStats.mvtRange + 1);
                MovementTile.path = new List<Tile>() { this.currentTile};
            }
        }

    }

    public GUIStyle getGUIStyle(bool active)
    {
        GUIStyle style = new GUIStyle();
        style.normal.background = menuItem;
        style.alignment = TextAnchor.MiddleLeft;
        if (active)
        {
            style.hover.background = menuItemHovered;
            style.normal.textColor = Color.white;
            style.hover.textColor = Color.white;
        }
        else
        {
            style.hover.background = menuItem;
            style.normal.textColor = Color.gray;
            style.hover.textColor = Color.gray;
        }
        return style;
    }

    private int Angle(Vector2 from, Vector2 to) {
        Vector2 diff = to - from;
        int output = (int)(Mathf.Atan2(diff.y, diff.x)* 57.2957795131f);
        if (output < 0)
            output += 360;
        return output;
    }

    public void SetTile(Tile newTile) {
        transform.position = newTile.transform.position;
        currentTile.currentUnit = null;
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

    // Phase Change Methods //
    public void MakeOpen() {
        phase = UnitTurn.Open;
        SelectionController.selectionMode = SelectionMode.Open;
        spriteRenderer.color = Color.white;
        spriteRenderer.sprite = sprites.idle[facing];
        animator.runtimeAnimatorController = sprites.idleAnim[facing];
    }

    public void MakeMoving() {
        previousTile = currentTile;
        phase = UnitTurn.Moving;
    }

    public void MakeChoosingAction() {
        phase = UnitTurn.ChoosingAction;

    }

    public IEnumerator PerformAttack()
    {
        spriteRenderer.sprite = sprites.attack[facing];
        animator.runtimeAnimatorController = sprites.attackAnim[facing];
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length/animator.speed);
        SelectionController.target.unitStats.health -= SelectionController.selectedUnit.unitStats.attack;
        if (HexMap.GetAttackTiles(SelectionController.target.currentTile).Contains(currentTile))
        {
            SelectionController.selectedUnit.unitStats.health -= max((unitStats.attack * 2) / 3, 1);
        }
        Image health = SelectionController.target.transform.Find("HealthBar").GetComponent<Image>();
        health.fillAmount = (float)SelectionController.target.unitStats.health/ (float)SelectionController.target.unitStats.maxHealth;
        health = transform.Find("HealthBar").GetComponent<Image>();
        health.fillAmount = Mathf.Max(0,(float)unitStats.health / (float)unitStats.maxHealth);
        if (unitStats.health <= 0)
            Destroy(gameObject);
        if (SelectionController.target.unitStats.health <= 0)
            Destroy(SelectionController.target.gameObject);
        MakeDone();
    }
    private int max(int a, int b) { return a > b ? a : b; }

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
        SelectionController.selectionMode = SelectionMode.Open;
    }
    ///////////////////////////
    // Pathing Methods //
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
        return (System.Math.Abs(-b.position.row+a.position.row-b.position.col+a.position.col)+System.Math.Abs(a.position.row- b.position.row)+System.Math.Abs(a.position.col - b.position.col))/2;
    }

    // TODO: change to actually using an attack range stat, currently moves right next to unit
    public bool HasEnemyInRange() {
        List<Tile> neighbors = HexMap.GetNeighbors(currentTile);
        foreach (Tile neighbor in neighbors) {
            if (neighbor && neighbor.currentUnit && neighbor.currentUnit.isPlayerUnit) {
                return true;
            }
        }
        return false;
    }
}

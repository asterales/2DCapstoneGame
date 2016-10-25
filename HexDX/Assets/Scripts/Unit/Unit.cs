﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

// this class represents a Unit and stores its data

public class Unit : MonoBehaviour {    
    public Tile currentTile;
    public UnitStats unitStats;
    public UnitSprites sprites;
    public List<Vector2> attackablePositions;

    private Queue<Tile> path;
    public UnitTurn phase;
    public int facing;
    public int experience;
    public int veterancy;
    public MovementDifficulty mvtDifficulty;

    public static PlayerBattleController player;
    public static AIBattleController ai;
    public SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Animator animator;
    private Tile lastTile;
    private int type; // we may want to represent types by something else
    private readonly float maxMovement = 0.13f;
    private UnitFacing facingBonus;
    private UnitSounds sounds;
    private static bool attackLock = false;

    // temporary storage for scripted stuff
    ScriptedMove scriptedMove;

    // Properties for shorthand access to stats, includes tile modifiers
    public int MvtRange { get { return unitStats.mvtRange + (currentTile ? currentTile.tileStats.mvtModifier : 0); } }
    public int MaxHealth { get { return unitStats.maxHealth; } }
    public int Health {
        get { return unitStats.health; }
        set { unitStats.health = value; }
    }
    public int Attack { get { return unitStats.attack + (currentTile ? currentTile.tileStats.attackModifier : 0); } }
    public int Defense { get { return unitStats.defense + (currentTile ? currentTile.tileStats.defenseModifier : 0); } }
    public int Power { get { return unitStats.power + (currentTile ? currentTile.tileStats.powerModifier : 0); } }
    public int Resistance { get { return unitStats.resistance + (currentTile ? currentTile.tileStats.resistanceModifier : 0); } }

    void Awake() {
        unitStats = this.gameObject.GetComponent<UnitStats>();
        facingBonus = this.gameObject.GetComponent<UnitFacing>();
        sprites = this.gameObject.GetComponent<UnitSprites>();
        sounds = this.gameObject.GetComponent<UnitSounds>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
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
        if (spriteRenderer == null)
        {
            Debug.Log("Unit Needs SpriteRenderer to be defined -> Unit.cs");
        }
        ////////////////////////
    }

    // Use this for initialization
    void Start () {
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

    public void InitForBattle() {
        player = GameObject.FindGameObjectWithTag("Map").GetComponent<BattleController>().player;
        ai = GameObject.FindGameObjectWithTag("Map").GetComponent<BattleController>().ai;
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
                if (lastTile!=currentTile)
                    CheckZonesOfControl();
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

    private void CheckZonesOfControl()
    {
        if (IsPlayerUnit())
        {
            foreach (Unit unit in ai.units)
            {
                if (HexMap.GetAttackTiles(unit).Contains(lastTile) && unit.phase == UnitTurn.Open && Health>0 && unit.Health>0)
                {
                    unit.MakeAttacking();
                    Timing.RunCoroutine(unit.DoAttack(this, 0.8f));
                }
            }
        }
        else
        {
            foreach (Unit unit in player.units)
            {
                if (HexMap.GetAttackTiles(unit).Contains(lastTile) && unit.phase == UnitTurn.Open && Health>0 && unit.Health>0)
                {
                    unit.MakeAttacking();
                    Timing.RunCoroutine(unit.DoAttack(this, 0.8f));
                }
            }
        }
    }

    public void SetFacingSprites() {
        int face = (facing + 1)%6;
        switch (phase) {
            case UnitTurn.Moving:
                if (audioSource != null) {
                    audioSource.Stop();
                    audioSource.clip = sounds.movement;
                    audioSource.Play();
                }
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
                if (audioSource != null) {
                    audioSource.Stop();
                    audioSource.clip = sounds.attacking;
                    audioSource.Play();                    
                }
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
                if(audioSource != null) {
                    audioSource.Pause();
                }
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
        if (currentTile && currentTile.currentUnit == this) {
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
        ////// DEBUG CODE FOR THE TUTORIAL LEVEL //////
        if (spriteRenderer == null) return;
        spriteRenderer.color = Color.white;
        SetFacingSprites();
    }

    public void SetPhase(UnitTurn phase)
    {
        switch (phase) {
            case UnitTurn.Attacking: MakeAttacking(); break;
            case UnitTurn.Moving: MakeMoving(); break;
            case UnitTurn.Open: MakeOpen(); break;
            case UnitTurn.Done: MakeDone(); break;
            case UnitTurn.ChoosingAction: MakeChoosingAction(); break;
        }


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
        if (this.IsPlayerUnit() && !SelectionController.TakingAIInput())
            HexMap.ShowAttackTiles(this);
        else if (!this.IsPlayerUnit())
        {
            HexMap.ShowAttackTiles(this);
        }
    }

    public void MakeFacing() {
        phase = UnitTurn.Facing;
    }

    public void MakeDone() {
        if (this != null && gameObject)
        {
            phase = UnitTurn.Done;
            HexMap.ClearAttackTiles();
            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
            SetFacingSprites();
        }
    }

    public void Die()
    {
        MakeDone();
        path = null;
        lastTile = null;
        this.gameObject.AddComponent<UnitDeath>();

    }
   

    public IEnumerator<float> PerformAttack(Unit target) {
        if (target && target.Health>0 && Health>0)
            Timing.RunCoroutine(DoAttack(target, 1.0f));
        //if (target && target.gameObject && target.HasInAttackRange(this))
        yield return Timing.WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length / 5.0f);
        if (target && target.Health > 0 && target.HasInAttackRange(this) && target.phase==UnitTurn.Open && Health>0)
        {
            Debug.Log(target.phase);
            spriteRenderer.color = Color.red;
            target.MakeAttacking();
            Timing.RunCoroutine(target.DoAttack(this, .8f));
        }

    }

    public IEnumerator<float> DoAttack(Unit target, float modifier)
    {
        SetFacingSprites();
        int healthStart = target.Health;
        int basedamage = (int)(Attack * (50.0f / (50.0f + (float)target.Defense)));
        int damage = basedamage;
        string indicatorText = "-" + (int)(basedamage * modifier); ;
        if (target.phase != UnitTurn.Moving)
        {
            if (target.facing == facing)
            {
                damage = basedamage * 2;
                indicatorText = "-" + (int)(basedamage * modifier) + "\nSneak!\n-" + (int)((damage - basedamage) * modifier);
            }
            else if (Mathf.Abs(target.facing - facing) == 1 || Mathf.Abs(target.facing - facing) == 5)
            {
                damage = basedamage * 3 / 2;
                indicatorText = "-" + (int)(basedamage * modifier) + "\nFlank!\n-" + (int)((damage - basedamage) * modifier);
            }
        }
        target.Health -= (int)(damage * modifier);
        if (target.Health <= 0)
        {
            target.path = new Queue<Tile>();
            target.lastTile = null;
            target.MakeDone();
            target.spriteRenderer.color = Color.white;
        }
        yield return Timing.WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length / 5.0f);
        Timing.RunCoroutine(finishAttack());
        GameObject indicator = new GameObject();
        indicator.AddComponent<DamageIndicator>().SetDamage(indicatorText);
        indicator.transform.position = target.transform.position + new Vector3(-1f, 6f, 0f);
        target.DrawHealth();
        if (healthStart-(int)(damage*modifier)<=0)
        {
            if (target.scriptedMove != null)
            {
                target.scriptedMove.FinishEvent();
                target.scriptedMove = null;
            }
            target.Die();
        }
    }

    public void DrawHealth()
    {
        Image healthBar = this.transform.Find("HealthBar").GetComponent<Image>(); // Find() is expensive
        healthBar.fillAmount = (float)Health / (float)MaxHealth;
    }

    public IEnumerator<float> finishAttack()
    {
        yield return Timing.WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length / 5.0f);
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
            List<Tile> exploredNodes = new List<Tile>();
            int t = Search(dest,currentTile, 0, bound, ref shortestPath, ref exploredNodes);
            if (t == -1) {
                shortestPath.Add(dest);
                break;
            }
            if (t == int.MaxValue) { // wat
                Debug.Log("Impossible to reach");
                return new List<Tile>();
            }
            bound = t;
            shortestPath = new List<Tile>();
        }
        if (shortestPath.Count == 0)
            shortestPath.Add(currentTile);
        return shortestPath;
    }

    private int Search(Tile node, Tile dest, int g, int bound, ref List<Tile> currentPath, ref List<Tile> exploredNodes) {
        int f = g + HexMap.Cost(node, dest);
        if (f > bound) {
            return f;
        }
        if (node == dest) {
            return -1;
        }
        exploredNodes.Add(node);
        int min = int.MaxValue;
        foreach (Tile neighbor in HexMap.GetNeighbors(node)) {
            if (!exploredNodes.Contains(neighbor) && (neighbor == dest || CanPathThrough(neighbor))) { // hack to allow bfs to terminate even if dest tile is not pathable
                int t = Search(neighbor, dest, g + 1, bound, ref currentPath, ref exploredNodes);
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

    // AI pathfinding methods
    public int CanReachTileAndAttack(Tile dest, int direction)
    {
        // return values:
        // -- 0 cant reach
        // -- 1 can reach
        // -- 2 can flank
        // -- 3 can counter
        // to be implemented
        // i think this has already been implemented... future note: look at HasInAtackRange
        return 0;
    }

    public static void SaveAllStates()
    {
        UnitState.ClearStates();
        foreach (Unit unit in ai.units)
        {
            UnitState.SaveState(unit);
        }
        foreach (Unit unit in player.units)
        {
            UnitState.SaveState(unit);
        }
    }

    public List<Tile> GetAllMoveTilesInRange()
    {
        // to be implemented
        return null;
    }
}


public class UnitState {
    public UnitTurn phase;
    public Tile tile;
    public int facing;
    public int health;
    public static Dictionary<Unit, UnitState> savedStates = new Dictionary<Unit, UnitState>();
    public UnitState(UnitTurn phase, Tile tile, int facing, int health)
    {
        this.phase = phase;
        this.tile = tile;
        this.facing = facing;
        this.health = health;
    }

    public static void SaveState(Unit unit)
    {
        savedStates[unit] = new UnitState(unit.phase, unit.currentTile, unit.facing, unit.Health);
    }

    public static void RestoreStates()
    {
        foreach (Unit unit in savedStates.Keys)
        {
            if (unit)
            {
                unit.SetTile(savedStates[unit].tile);
                unit.facing = savedStates[unit].facing;
                unit.Health = savedStates[unit].health;
                unit.SetPhase(savedStates[unit].phase);
                unit.DrawHealth();
            }
        }
    }

    public static void ClearStates()
    {
        savedStates = new Dictionary<Unit, UnitState>();
    }
}

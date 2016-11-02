using UnityEngine;
using System;
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
    public float moveSpeed = 0.13f;

    private Queue<Tile> path;
    public UnitTurn phase;
    public int facing;

    public static PlayerBattleController player;
    public static AIBattleController ai;
    public SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Animator animator;
    private Tile lastTile;
    private int type; // we may want to represent types by something else
    private UnitFacing facingBonus;
    private UnitSounds sounds;
    private static bool attackLock = false;

    private Image v1, v2, v3, v4, v5;
    // temporary storage for scripted stuff
    ScriptedMove scriptedMove;

    // Properties for shorthand access to stats, includes tile modifiers
    public int MvtRange { get { return unitStats.mvtRange + (currentTile ? currentTile.tileStats.mvtModifier : 0); } }
    public int MaxHealth { get { return unitStats.maxHealth; } }
    public int Health {
        get { return unitStats.health; }
        set { unitStats.health = value; }
    }
    public int Experience
    {
        get { return unitStats.experience; }
        set { unitStats.experience = value; }
    }
    public int Veterancy
    {
        get { return unitStats.veterancy; }
        set { unitStats.veterancy = value; }
    }
    public int Attack { get { return unitStats.attack + (currentTile ? currentTile.tileStats.attackModifier : 0); } }
    public int Defense { get { return unitStats.defense + (currentTile ? currentTile.tileStats.defenseModifier : 0); } }
    public int Power { get { return unitStats.power + (currentTile ? currentTile.tileStats.powerModifier : 0); } }
    public int Resistance { get { return unitStats.resistance + (currentTile ? currentTile.tileStats.resistanceModifier : 0); } }

    void Awake() {
        unitStats = GetComponent<UnitStats>();
        facingBonus = GetComponent<UnitFacing>();
        sprites = GetComponent<UnitSprites>();
        sounds = GetComponent<UnitSounds>();
        audioSource = GetComponent<AudioSource>();
        path = new Queue<Tile>();

        //veterancy images
        try
        {
            v1 = this.transform.GetChild(2).GetChild(0).GetComponent<Image>();
            v2 = this.transform.GetChild(2).GetChild(1).GetComponent<Image>();
            v3 = this.transform.GetChild(2).GetChild(2).GetComponent<Image>();
            v4 = this.transform.GetChild(2).GetChild(3).GetComponent<Image>();
            v5 = this.transform.GetChild(2).GetChild(4).GetComponent<Image>();
            DrawVeterancy();
        } catch (UnityException e)
        {
            Debug.Log(e.StackTrace);
        }

        facing = 0;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

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
        DrawHealth();
        player = BattleControllerManager.instance.player;
        ai = BattleControllerManager.instance.ai;
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
                transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed);
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


    public void AddExp(int exp)
    {
        if (Veterancy < 3)
        {
            Experience += exp;
            if (Experience > (Veterancy + 1) * 100)
            {
                Experience = Experience%((Veterancy+1)*100);
                Veterancy += 1;
                LevelUp();
            }
        }
        DrawVeterancy();
    }
    public void DrawVeterancy()
    {
        SpriteRenderer b1, b2, b3, b4, b5;
        b1 = v1.GetComponent<SpriteRenderer>();
        b2 = v2.GetComponent<SpriteRenderer>();
        b3 = v3.GetComponent<SpriteRenderer>();
        b4 = v4.GetComponent<SpriteRenderer>();
        b5 = v5.GetComponent<SpriteRenderer>();
        if (IsPlayerUnit())
        {
            switch (Veterancy)
            {
                case 0:
                    v1.fillAmount = (float)Experience / 100.0f;
                    v2.fillAmount = 0;
                    v3.fillAmount = 0;
                    v4.fillAmount = 0;
                    v5.fillAmount = 0;
                    b1.color = Color.white;
                    b2.color = Color.clear;
                    b3.color = Color.clear;
                    b4.color = Color.clear;
                    b5.color = Color.clear;
                    break;
                case 1:
                    v1.fillAmount = 0;
                    v2.fillAmount = 0;
                    v3.fillAmount = 0;
                    v4.fillAmount = 1;
                    v5.fillAmount = (float)Experience / 200.0f;
                    b1.color = Color.clear;
                    b2.color = Color.clear;
                    b3.color = Color.clear;
                    b4.color = Color.white;
                    b5.color = Color.white;
                    break;
                case 2:
                    v1.fillAmount = 1;
                    v2.fillAmount = 1;
                    v3.fillAmount = (float)Experience / 300.0f;
                    v4.fillAmount = 0;
                    v5.fillAmount = 0;
                    b1.color = Color.white;
                    b2.color = Color.white;
                    b3.color = Color.white;
                    b4.color = Color.clear;
                    b5.color = Color.clear;
                    break;
                default:
                    v1.fillAmount = 1;
                    v2.fillAmount = 1;
                    v3.fillAmount = 1;
                    v4.fillAmount = 0;
                    v5.fillAmount = 0;
                    b1.color = Color.white;
                    b2.color = Color.white;
                    b3.color = Color.white;
                    b4.color = Color.clear;
                    b5.color = Color.clear;
                    break;
            }
        }
        else
        {
            switch (Veterancy)
            {
                case 0:
                    v1.fillAmount = 0;
                    v2.fillAmount = 0;
                    v3.fillAmount = 0;
                    v4.fillAmount = 0;
                    v5.fillAmount = 0;
                    break;
                case 1:
                    v1.fillAmount = 1;
                    v2.fillAmount = 0;
                    v3.fillAmount = 0;
                    v4.fillAmount = 0;
                    v5.fillAmount = 0;
                    break;
                case 2:
                    v1.fillAmount = 0;
                    v2.fillAmount = 0;
                    v3.fillAmount = 0;
                    v4.fillAmount = 1;
                    v5.fillAmount = 1;
                    break;
                default:
                    v1.fillAmount = 1;
                    v2.fillAmount = 1;
                    v3.fillAmount = 1;
                    v4.fillAmount = 0;
                    v5.fillAmount = 0;
                    break;
            }
            b1.color = Color.clear;
            b2.color = Color.clear;
            b3.color = Color.clear;
            b4.color = Color.clear;
            b5.color = Color.clear;

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
   
    public void LevelUp()
    {
        unitStats.attack = (int)(Attack * 1.3f);
        unitStats.defense = (int)(Defense * 1.3f);
        unitStats.power = (int)(Power * 1.3f);
        unitStats.resistance = (int)(Resistance * 1.3f);
        unitStats.health += (int)(MaxHealth * 0.3f);
        unitStats.maxHealth = (int)(MaxHealth* 1.3f);
        DrawHealth();
    }

    public void LevelDown()
    {
        unitStats.attack = (int)(Attack / 1.3f);
        unitStats.defense = (int)(Defense / 1.3f);
        unitStats.power = (int)(Power / 1.3f);
        unitStats.resistance = (int)(Resistance / 1.3f);
        unitStats.maxHealth = (int)(MaxHealth / 1.3f);
        unitStats.health -= (int)(MaxHealth * 0.3f);
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

        if (IsPlayerUnit())
        {
            AddExp((int)(damage * modifier));
        }
        else
        {
            target.AddExp((int)(0.5f*damage * modifier));
        }
        yield return Timing.WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length / 5.0f);
        Timing.RunCoroutine(finishAttack());
        GameObject indicator = new GameObject();
        indicator.AddComponent<DamageIndicator>().SetDamage(indicatorText);
        indicator.transform.position = target.transform.position + new Vector3(-0.5f, 3f, 0f);
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
                && (!tile.currentUnit || IsPlayerUnit() == tile.currentUnit.IsPlayerUnit());
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
            if (t == int.MaxValue) {
                //Debug.Log("Impossible to reach"); commenting out, because its flooding the logger
                return new List<Tile>();
            }
            bound = t;
            shortestPath = new List<Tile>();
        }
        if (shortestPath.Count == 0)
            shortestPath.Add(currentTile);
        return shortestPath;
    }

    public int GetShortestPathLength(Tile dest, Tile current)
    {
        int bound = HexMap.Cost(dest, current);
        List<Tile> shortestPath = new List<Tile>();
        while (true)
        {
            List<Tile> exploredNodes = new List<Tile>();
            int t = Search(dest, current, 0, bound, ref shortestPath, ref exploredNodes);
            if (t == -1)
            {
                shortestPath.Add(dest);
                break;
            }
            if (t == int.MaxValue)
            {
                //Debug.Log("Impossible to reach"); commenting out, because its flooding the logger
                return int.MaxValue;
            }
            bound = t;
            shortestPath = new List<Tile>();
        }
        if (shortestPath.Count == 0)
            shortestPath.Add(current);
        return shortestPath.Count;
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
        return GetComponent<UnitAI>() == null;
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
}


public class UnitState {
    public UnitTurn phase;
    public Tile tile;
    public int facing;
    public int health;
    public int experience;
    public int veterancy;
    public static Dictionary<Unit, UnitState> savedStates = new Dictionary<Unit, UnitState>();
    public UnitState(UnitTurn phase, Tile tile, int facing, int health, int experience, int veterancy)
    {
        this.phase = phase;
        this.tile = tile;
        this.facing = facing;
        this.health = health;
        this.experience = experience;
        this.veterancy = veterancy;
    }

    public static void SaveState(Unit unit)
    {
        savedStates[unit] = new UnitState(unit.phase, unit.currentTile, unit.facing, unit.Health, unit.Experience, unit.Veterancy);
    }

    public static void RestoreStates()
    {
        foreach (Unit unit in savedStates.Keys)
        {
            if (unit)
            {
                if (unit.Veterancy > savedStates[unit].veterancy)
                    unit.LevelDown();
                unit.SetTile(savedStates[unit].tile);
                unit.facing = savedStates[unit].facing;
                unit.Health = savedStates[unit].health;
                unit.Experience = savedStates[unit].experience;
                unit.Veterancy = savedStates[unit].veterancy;
                unit.Health = savedStates[unit].health;
                unit.SetPhase(savedStates[unit].phase);
                unit.DrawHealth();
                unit.DrawVeterancy();
            }
        }
    }

    public static void ClearStates()
    {
        savedStates = new Dictionary<Unit, UnitState>();
    }
}

﻿using UnityEngine;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

// this class represents a Unit and stores its data
/* facing key
     2 1
    3   0
     4 5 
*/

public class Unit : MonoBehaviour {

    public Tile currentTile;
    public UnitTurn phase;
    public int facing;
    public List<Vector2> attackablePositions;
    public UnitSprites sprites;
    public float moveSpeed = 0.13f;
    public PlayerBattleController player;
    public AIBattleController ai;
    public SpriteRenderer spriteRenderer;

    private SelectionController sc;
    private AudioSource audioSource;
    private Animator animator;
    private Tile lastTile;
    private UnitStats unitStats;
    private UnitSounds sounds;
    private Image v1, v2, v3, v4, v5;
    private Queue<Tile> path;
    private ScriptedMove scriptedMove;    // temporary storage for scripted stuff

    // Properties for shorthand access to stats, includes tile modifiers
    public int MaxHealth { get { return unitStats.maxHealth; } }
    public int Health { get { return unitStats.health; } set { unitStats.health = value; } }
    public int Experience { get { return unitStats.experience; } set { unitStats.experience = value; } }
    public int Veterancy { get { return unitStats.veterancy; } set { unitStats.veterancy = value; } }
    public float ZOCModifer { get { return unitStats.zocmodifier; } set { unitStats.zocmodifier = value; } }
    public int Attack { get { return unitStats.attack + (currentTile ? currentTile.tileStats.attackModifier : 0); } }
    public int Defense { get { return unitStats.defense + (currentTile ? currentTile.tileStats.defenseModifier : 0); } }
    public int Power { get { return unitStats.power + (currentTile ? currentTile.tileStats.powerModifier : 0); } }
    public int Resistance { get { return unitStats.resistance + (currentTile ? currentTile.tileStats.resistanceModifier : 0); } }
    public int MvtRange { get { return unitStats.mvtRange; } }
    public string ClassName { get { return unitStats.className; } set { unitStats.className = value; } }


    void Awake() {
        facing = 0;
        scriptedMove = null;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        unitStats = GetComponent<UnitStats>();
        sprites = GetComponent<UnitSprites>();
        sounds = GetComponent<UnitSounds>();
        audioSource = GetComponent<AudioSource>();
        path = new Queue<Tile>();

        //veterancy images
        try {
            v1 = transform.GetChild(2).GetChild(0).GetComponent<Image>();
            v2 = transform.GetChild(2).GetChild(1).GetComponent<Image>();
            v3 = transform.GetChild(2).GetChild(2).GetComponent<Image>();
            v4 = transform.GetChild(2).GetChild(3).GetComponent<Image>();
            v5 = transform.GetChild(2).GetChild(4).GetComponent<Image>();
            DrawVeterancy();
        } catch (UnityException e) {
            Debug.Log(e.StackTrace);
        }

        ////// DEBUG CODE //////
        if (unitStats == null) {
            Debug.Log("Unit Needs Unit Stats to be defined -> Unit.cs");
        }
        if (spriteRenderer == null) {
            Debug.Log("Unit Needs SpriteRenderer to be defined -> Unit.cs");
        }
        ////////////////////////
    }

    // Use this for initialization
    void Start() {
        MakeOpen();
        DrawVeterancy();
        DrawHealth();
    }

    void Update() {
        animator.speed = SpeedController.speed;
        switch (phase) {
            case UnitTurn.Moving:
                Move();
                break;
            case UnitTurn.Facing:
                Face();
                break;
        }
    }

    public void InitForBattle() {
        player = BattleManager.instance.player;
        ai = BattleManager.instance.ai;
        sc = SelectionController.instance;
    }

    private void Move() {
        if (path.Count > 0) {
            // if player turn, disable the players ability to select
            if (sc.TakingInput()) {
                sc.mode = SelectionMode.Moving;
            }
            lastTile = path.Peek();
            Vector3 destination = lastTile.transform.position;
            // if not at tile, move to tiles
            if (Vector3.Distance(transform.position, destination) > 0.01f*SpeedController.speed) {
                transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed*SpeedController.speed);
            } else { // if at a new tile...
                CheckZonesOfControl();
                if (path.Count == 1) { // if this is the last tile, set the new tile and move to facing phase
                    SetTile(path.Dequeue());
                    // re-enable the players ability to select for facing
                    if (sc.mode == SelectionMode.Moving) {
                        sc.mode = SelectionMode.Facing;
                    }
                    MakeFacing();
                    if (scriptedMove != null) {
                        scriptedMove.FinishEvent();
                        scriptedMove = null;
                    }
                } else { // if its not the last tile, set the unit to face the next tile
                    path.Dequeue();
                    facing = HexMap.GetNeighbors(lastTile).IndexOf(path.Peek());
                    SetFacingSprites();
                }
            }
        }
    }

    private void CheckZonesOfControl() {
        List<Unit> enemyUnits = IsPlayerUnit() ? ai.units : player.units;
        foreach (Unit unit in enemyUnits) {
            if (HexMap.GetAttackTiles(unit).Contains(lastTile) && unit.phase == UnitTurn.Open && Health > 0 && unit.Health > 0) {
                unit.MakeAttacking();
                Timing.RunCoroutine(unit.DoAttack(this, unit.ZOCModifer));
            }
        }
    }

    public void SetFacingSprites() {
        int face = (facing + 1) % 6;
        switch (phase) {
            case UnitTurn.Moving:
                if (audioSource != null) {
                    audioSource.Stop();
                    audioSource.clip = sounds.movement;
                    audioSource.Play();
                }
                if (face < 3) {
                    spriteRenderer.flipX = false;
                    spriteRenderer.sprite = sprites.walking[(facing + 3) % 3];
                    animator.runtimeAnimatorController = sprites.walkingAnim[(facing + 3) % 3];
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
                if (audioSource != null) {
                    audioSource.Pause();
                }
                if (face < 3) {
                    spriteRenderer.flipX = false;
                    spriteRenderer.sprite = sprites.idle[(facing + 3) % 3];
                    animator.runtimeAnimatorController = sprites.idleAnim[(facing + 3) % 3];
                } else {
                    spriteRenderer.flipX = true;
                    spriteRenderer.sprite = sprites.walking[(6 - facing) % 3];
                    animator.runtimeAnimatorController = sprites.idleAnim[(6 - facing) % 3];
                }
                break;
        }
    }


    public void AddExp(int exp) {
        if (Veterancy < 3) {
            Experience += exp;
            if (Experience > 100+(Veterancy) * 50) {
                Experience = Experience % (100+Veterancy*50);
                Veterancy += 1;
                LevelUp();
            }
        }
        DrawVeterancy();
    }

    public void LevelUp() {
        unitStats.LevelUp();
        DrawHealth();
    }

    public void LevelDown() {
        unitStats.LevelDown();
        DrawHealth();
    }

    public void DrawVeterancy() {
        SpriteRenderer b1, b2, b3, b4, b5;
        b1 = v1.GetComponent<SpriteRenderer>();
        b2 = v2.GetComponent<SpriteRenderer>();
        b3 = v3.GetComponent<SpriteRenderer>();
        b4 = v4.GetComponent<SpriteRenderer>();
        b5 = v5.GetComponent<SpriteRenderer>();
        if (IsPlayerUnit()) {
            switch (Veterancy) {
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
        } else {
            switch (Veterancy) {
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

    public void RemoveFromMap() {
        if (enabled) {
            if (currentTile && currentTile.currentUnit == this) {
                currentTile.currentUnit = null;
            }
            currentTile = null;
        }
    }

    public void SetTile(Tile newTile) {
        if (enabled) {
            transform.position = newTile.transform.position;
            transform.parent = newTile.transform;
            if (currentTile && currentTile.currentUnit == this) {
                currentTile.currentUnit = null;
            }
            newTile.currentUnit = this;
            currentTile = newTile;
        }
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

    public int GetFacing(Vector2 directionVec)
    {
        int facing;
        int angle = Angle(new Vector2(1, 0), directionVec);
        if (angle < 18 || angle > 342)
        {
            facing = 0;
        }
        else if (angle < 91)
        {
            facing = 1;
        }
        else if (angle < 164)
        {
            facing = 2;
        }
        else if (angle < 198)
        {
            facing = 3;
        }
        else if (angle < 271)
        {
            facing = 4;
        }
        else
        {
            facing = 5;
        }
        return facing;
    }


    private int Angle(Vector2 from, Vector2 to) {
        Vector2 diff = to - from;
        int output = (int)(Mathf.Atan2(diff.y, diff.x) * 57.2957795131f);
        if (output < 0) {
            output += 360;
        }
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

    public void SetPhase(UnitTurn phase) {
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

    public void MakeChoosingAction() {
        foreach (Unit unit in ai.units) {
            if (unit != this && unit.phase != UnitTurn.Open && unit.phase != UnitTurn.Done) {
                return;
            }
        }
        foreach (Unit unit in player.units) {
            if (unit!=this && unit.phase != UnitTurn.Open && unit.phase != UnitTurn.Done) {
                return;
            }
        }
        phase = UnitTurn.ChoosingAction;
        if (IsPlayerUnit()) {
            GameManager.instance.PlayCursorSfx();
        }
    }

    public void MakeAttacking() {
        spriteRenderer.color = Color.white;
        phase = UnitTurn.Attacking;
        if (IsPlayerUnit() && !sc.TakingAIInput()) {
            HexMap.ShowAttackTiles(this);
        } else if (!IsPlayerUnit()) {
            HexMap.ShowAttackTiles(this);
        }
    }

    public void MakeFacing() {
        phase = UnitTurn.Facing;
    }

    public void MakeDone() {
        if (this != null && gameObject) {
            phase = UnitTurn.Done;
            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
            SetFacingSprites();
            if (this == sc.selectedUnit) {
                HexMap.ClearAttackTiles();
            }
        }
    }

    public void Die() {
        MakeDone();
        BattleController.instance.ResetColors();
        path = null;
        lastTile = null;
        RemoveFromMap();
        gameObject.AddComponent<UnitDeath>();
    }

    public IEnumerator<float> PerformAttack(Unit target) {
        if (target.enabled && target.Health>0 && Health>0) {
            Timing.RunCoroutine(DoAttack(target, 1.0f));
        }
        //if (target && target.gameObject && target.HasInAttackRange(this))
        yield return Timing.WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length / 5.0f/SpeedController.speed);
        if (target.enabled && target.Health > 0 && target.HasInAttackRange(this) && target.phase==UnitTurn.Open && Health > 0) {
            target.MakeAttacking();
            Timing.RunCoroutine(target.DoAttack(this, target.ZOCModifer));
        }
        else
        {
            PlayerUIDrawer.instance.SetPreview(0);
        }

    }

    public IEnumerator<float> DoAttack(Unit target, float modifier) {
        SetFacingSprites();
        int healthStart = target.Health;
        int basedamage = (int)(Attack * (50.0f / (50.0f + (float)target.Defense)))+ (int)(Power * (50.0f / (50.0f + (float)target.Resistance)));
        int damage = basedamage;
        int attackdirection = GetFacing(target.transform.position - transform.position);
        string indicatorText = "-" + (int)(basedamage * modifier);
        float facingModifier = 1.0f; ;
        HexMap.ClearAllTiles();
        if (target.phase != UnitTurn.Moving) {
            if (target.facing == attackdirection) {
                facingModifier = 2.0f;
                damage = basedamage * 2;
                indicatorText = "-" + (int)(basedamage * modifier) + "\nSneak!\n-" + (int)((damage - basedamage) * modifier);
            } else if (Mathf.Abs(target.facing - attackdirection) == 1 || Mathf.Abs(target.facing - attackdirection) == 5) {
                facingModifier = 1.5f;
                damage = basedamage * 3 / 2;
                indicatorText = "-" + (int)(basedamage * modifier) + "\nFlank!\n-" + (int)((damage - basedamage) * modifier);
            }
        }
        target.Health -= (int)(damage * modifier);
        if (target.IsPlayerUnit())
            PlayerUIDrawer.instance.damagePreview -= (int)(damage * modifier);
        else
            EnemyUIDrawer.instance.damagePreview -= (int)(damage * modifier);
        if (target.Health <= 0) {
            target.Health = 0;
            target.path = new Queue<Tile>();
            target.lastTile = null;
            target.MakeDone();
            target.spriteRenderer.color = Color.white;
        }

        if (IsPlayerUnit()) {
            AddExp((int)(25 * modifier *facingModifier* (1+.5f*target.Veterancy)));
        }

        yield return Timing.WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length / 5.0f/SpeedController.speed);
        Timing.RunCoroutine(FinishAttack());
        GameObject indicator = new GameObject();
        indicator.AddComponent<DamageIndicator>().SetDamage(indicatorText);
        indicator.transform.position = target.transform.position + new Vector3(-0.5f, 3f, 0f);
        target.DrawHealth();
        if (healthStart-(int)(damage*modifier)<=0) {
            if (target.scriptedMove != null) {
                target.scriptedMove.FinishEvent();
                target.scriptedMove = null;
            }
            target.Die();
        }
    }

    public int GetDamage(Unit target, float modifier = 1.0f)
    {
        int basedamage =(int)(Attack * (50.0f / (50.0f + (float)target.Defense))) + (int)(Power * (50.0f / (50.0f + (float)target.Resistance)));
        int damage = (int)(basedamage * modifier);
        int attackdirection = GetFacing(target.transform.position - transform.position);
        if (target.phase != UnitTurn.Moving)
        {
            if (target.facing == attackdirection)
            {
                damage = (int)(basedamage * 2 * modifier);
            }
            else if (Mathf.Abs(target.facing - attackdirection) == 1 || Mathf.Abs(target.facing - attackdirection) == 5)
            {
                damage = (int)(basedamage * 3 * modifier) / 2;
            }
        }
        return damage;
    }

    public int GetDamage(Unit target, Tile dest, float modifier = 1.0f)
    {
        int basedamage = (int)(Attack * (50.0f / (50.0f + (float)target.Defense))) + (int)(Power * (50.0f / (50.0f + (float)target.Resistance)));
        int damage = (int)(basedamage * modifier);
        int attackdirection = GetFacing(target.transform.position - dest.transform.position);
        if (target.phase != UnitTurn.Moving)
        {
            if (target.facing == attackdirection)
            {
                damage = (int)(basedamage * 2 * modifier);
            }
            else if (Mathf.Abs(target.facing - attackdirection) == 1 || Mathf.Abs(target.facing - attackdirection) == 5)
            {
                damage = (int)(basedamage * 3 * modifier) / 2;
            }
        }
        return damage;
    }


    public void DrawHealth() {
        Image healthBar = transform.Find("HealthBar").GetComponent<Image>(); // Find() is expensive
        healthBar.fillAmount = (float)Health / (float)MaxHealth;
    }

    private IEnumerator<float> FinishAttack() {
        yield return Timing.WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length / 5.0f/SpeedController.speed);
        if (phase == UnitTurn.Attacking) {
            MakeDone();
        }
    }

    ///////////////////////////
    // Pathing Methods //
    public bool CanPathThrough(Tile tile) {
        return tile != null && tile.pathable 
                && (!tile.currentUnit || IsPlayerUnit() == tile.currentUnit.IsPlayerUnit());
    }

    public List<Tile> GetAccessibleTiles() {
        Queue<Tile> toCheck = new Queue<Tile>();
        Queue<int> dist = new Queue<int>();
        toCheck.Enqueue(currentTile);
        List<Tile> neighbors;
        List<Tile> tiles = new List<Tile>();
        while (toCheck.Count > 0) {
            Tile t = toCheck.Dequeue();
            if (CanPathThrough(t)) {
                if (t.currentUnit == null || t.currentUnit.IsPlayerUnit() == IsPlayerUnit()) {
                    if (!t.currentUnit) tiles.Add(t);
                    neighbors = HexMap.GetNeighbors(t);
                    foreach (Tile neighbor in neighbors) {
                        if (neighbor && !tiles.Contains(neighbor) && !toCheck.Contains(neighbor)) {
                            toCheck.Enqueue(neighbor);
                        }
                    }
                }
            }
        }
        return tiles;
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

    public int GetShortestPathLength(Tile dest, Tile current) {
        int bound = HexMap.Cost(dest, current);
        List<Tile> shortestPath = new List<Tile>();
        while (true) {
            int t = Search(dest, current, 0, bound, ref shortestPath);
            if (t == -1) {
                shortestPath.Add(dest);
                break;
            }
            if (t == int.MaxValue) {
                //Debug.Log("Impossible to reach"); commenting out, because its flooding the logger
                return int.MaxValue;
            }
            bound = t;
            shortestPath = new List<Tile>();
        }
        if (shortestPath.Count == 0)
            shortestPath.Add(current);
        return MovementTile.PathCost(shortestPath);
    }

    private int Search(Tile node, Tile dest, int g, int bound, ref List<Tile> currentPath) {
        int f = g + HexMap.Cost(node, dest);
        if (f > 9)
            return int.MaxValue;
        if (f > bound) {
            return f;
        }
        if (node == dest) {
            return -1;
        }
        int min = int.MaxValue;
        foreach (Tile neighbor in HexMap.GetNeighbors(node)) {
            if (neighbor == dest || CanPathThrough(neighbor)) { // hack to allow bfs to terminate even if dest tile is not pathable
                int t = Search(neighbor, dest, g + (int)neighbor.tileStats.mvtDifficulty, bound, ref currentPath);
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
        if (!other)
            return false;
        return HexMap.GetAttackTiles(this).Contains(other.currentTile);
    }

    public bool HasInTotalRange(Unit other) {
        if (!other)
            return false;
        return HexMap.GetTotalRange(this).Contains(other.currentTile);
    }

    public bool IsPlayerUnit() {
        return GetComponent<UnitAI>() == null;
    }

    public static void SaveAllStates() {
        UnitState.ClearStates();
        foreach (Unit unit in BattleManager.instance.ai.units) {
            UnitState.SaveState(unit);
        }
        foreach (Unit unit in BattleManager.instance.player.units) {
            UnitState.SaveState(unit);
        }
    }
}

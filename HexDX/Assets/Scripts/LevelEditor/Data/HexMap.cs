using UnityEngine;
using System.Collections.Generic;

public class HexMap : MonoBehaviour {
    private HexDimension hexDimension;
    public static List<List<Tile>> mapArray;
    public static Stack<Tile> showingMovementTiles;
    public static Stack<Tile> showingAttackTiles;
    public static Stack<GameObject> showingAttackOutlines;

    // Make a separate directions class/enum?
    public static readonly List<Vector2> directions = new List<Vector2> {
            new Vector2(0, 1), new Vector2(-1, 1), new Vector2(-1, 0), 
            new Vector2(0, -1), new Vector2(1, -1), new Vector2(1, 0)
        };

    void Awake() {
        hexDimension = GetComponent<HexDimension>();
        mapArray = new List<List<Tile>>();
        showingAttackTiles = new Stack<Tile>();
        showingMovementTiles = new Stack<Tile>();
        showingAttackOutlines = new Stack<GameObject>();
        ////// DEBUG CODE //////
        if (hexDimension == null) {
            Debug.Log("Error :: No Defined Hex Dimension for Hex Map - HexMap.cs");
        }
        transform.position = new Vector3(hexDimension.globalTopLeftX, hexDimension.globalTopLeftY, 0); // temp
        ////////////////////////
    }

    void OnDestroy() {
        ClearAllTiles();
    }

    public void ClearMap() {
        // remove and delete all current tiles
        for (int i = 0; i < mapArray.Count; i++) {
            while (mapArray[i].Count > 0) {
                int lastIndex = mapArray[i].Count - 1;
                Tile tile = mapArray[i][lastIndex];
                mapArray[i].RemoveAt(lastIndex);
                // maybe do more cleanup or caching here
                Destroy(tile);
            }
        }

        mapArray = new List<List<Tile>>();
    }

    public Unit[] GetUnitsOnMap() {
        return GetComponentsInChildren<Unit>();
    }


    public static List<Tile> GetMovementTiles(Unit unit) {
        Queue<Tile> toCheck = new Queue<Tile>();
        Queue<int> dist = new Queue<int>();
        toCheck.Enqueue(unit.currentTile);
        int distance = unit.MvtRange;
        dist.Enqueue(distance);
        List<Tile> neighbors;
        List<Tile> mvtTiles = new List<Tile>();
        while (toCheck.Count > 0) {
            Tile t = toCheck.Dequeue();
            distance = dist.Dequeue();
            if (unit.CanPathThrough(t) && distance>=0) {
                if (t.currentUnit == null || t.currentUnit.IsPlayerUnit() == unit.IsPlayerUnit()) {
                    mvtTiles.Add(t);
                    neighbors = GetNeighbors(t);
                    foreach (Tile neighbor in neighbors)
                    {
                        if (neighbor && !mvtTiles.Contains(neighbor) && !toCheck.Contains(neighbor))
                        {
                            toCheck.Enqueue(neighbor);
                            dist.Enqueue(distance - (int)neighbor.tileStats.mvtDifficulty);
                        }
                    }
                }
            }
        }
        return mvtTiles;
    }

    public static void ShowMovementTiles(Unit unit) {
        ClearAllTiles();
        List<Tile> mvtTiles = GetMovementTiles(unit);
        List<Tile> atkTiles = GetAttackTiles(unit);
        List<Tile> movementTiles = HexMap.GetMovementTiles(unit);
        int facing = unit.facing;
        Tile initial = unit.currentTile;
        foreach (Tile t in mvtTiles)
        {
            unit.currentTile = t;
            for (int i = 0; i < 6; i++)
            {
                unit.facing = i;
                if (!t.currentUnit || t.currentUnit == unit)
                    foreach (Tile tile in HexMap.GetAttackTiles(unit))
                    {
                        tile.ShowAttackSprite();
                    }
            }
        }
        unit.facing = facing;
        unit.currentTile = initial;
        foreach (Tile atkTile in atkTiles) {
            GameObject g = Instantiate(Resources.Load("Tiles/AttackableOutline")) as GameObject;
            g.transform.parent = atkTile.transform;
            g.transform.localPosition = new Vector3(0, 0, .0001f);
            showingAttackOutlines.Push(g);
        } 
        
        foreach (Tile mvtTile in mvtTiles) {
           mvtTile.ShowMovementTile();
        }
    }

    public static List<Tile> GetAttackTiles(Unit unit) {
        List<Tile> output = new List<Tile>();
        Vector2 rowDot = new Vector2(1, 1);
        Vector2 colDot = new Vector2(1, 1);
        switch (unit.facing) {
            case 0:
                rowDot = new Vector2(1, 0);
                colDot = new Vector2(0, 1);
                break;
            case 1:
                rowDot = new Vector2(0, -1);
                colDot = new Vector2(1, 1);
                break;
            case 2:
                rowDot = new Vector2(-1, -1);
                colDot = new Vector2(1, 0);
                break;
            case 3:
                rowDot = new Vector2(-1, 0);
                colDot = new Vector2(0, -1);
                break;
            case 4:
                rowDot = new Vector2(0, 1);
                colDot = new Vector2(-1, -1);
                break;
            case 5:
                rowDot = new Vector2(1, 1);
                colDot = new Vector2(-1, 0);
                break;
        }
        foreach (Vector2 pos in unit.attackablePositions) {
            try {
                output.Add(mapArray[unit.currentTile.position.row + (int)Vector2.Dot(pos,rowDot)][unit.currentTile.position.col + (int)Vector2.Dot(pos, colDot)]);
            }
            catch { }
        }
        return output;
    }

    public static List<Tile> GetAttackTiles(Tile tile, Unit unit, int direction)
    {
        List<Tile> output = new List<Tile>();
        Vector2 rowDot = new Vector2(1, 1);
        Vector2 colDot = new Vector2(1, 1);
        switch (direction)
        {
            case 0:
                rowDot = new Vector2(1, 0);
                colDot = new Vector2(0, 1);
                break;
            case 1:
                rowDot = new Vector2(0, -1);
                colDot = new Vector2(1, 1);
                break;
            case 2:
                rowDot = new Vector2(-1, -1);
                colDot = new Vector2(1, 0);
                break;
            case 3:
                rowDot = new Vector2(-1, 0);
                colDot = new Vector2(0, -1);
                break;
            case 4:
                rowDot = new Vector2(0, 1);
                colDot = new Vector2(-1, -1);
                break;
            case 5:
                rowDot = new Vector2(1, 1);
                colDot = new Vector2(-1, 0);
                break;
        }
        foreach (Vector2 pos in unit.attackablePositions)
        {
            try
            {
                output.Add(mapArray[tile.position.row + (int)Vector2.Dot(pos, rowDot)][tile.position.col + (int)Vector2.Dot(pos, colDot)]);
            }
            catch { }
        }
        return output;
    }

    public static List<Move> GetPossibleMoves(Unit unit)
    {
        List<Move> output = new List<Move>();
        List<Tile> mvtTiles = GetMovementTiles(unit);
        int facing = unit.facing;
        Tile initial = unit.currentTile;
        foreach (Tile t in mvtTiles)
        {
            unit.currentTile = t;
            if (!t.currentUnit || t.currentUnit == unit)
                for (int i = 0; i < 6; i++)
                {
                    unit.facing = i;
                    foreach (Tile tile in HexMap.GetAttackTiles(unit)) {
                        if (tile.currentUnit && tile.currentUnit.IsPlayerUnit()!=unit.IsPlayerUnit())
                            output.Add(new Move(unit, t, i, tile.currentUnit));
                        else
                            output.Add(new Move(unit, t, i, null));
                    }
                }
        }
        unit.facing = facing;
        unit.currentTile = initial;
        return output;
    }

    public static int GetPathDamage(List<Tile> path)
    {
        int damage = 0;
        List<Unit> attacking = new List<Unit>();
        foreach (Unit unit in BattleController.instance.player.units)
        {
            if (unit.phase == UnitTurn.Open)
            {
                bool inrange = false;
                foreach (Tile tile in path)
                {
                    if (HexMap.GetAttackTiles(unit).Contains(tile))
                    {
                        inrange = true;
                        attacking.Add(unit);
                        break;
                    }
                }
                if (!inrange || path.Count == 1)
                    attacking.Remove(unit);
                if (attacking.Contains(unit)) 
                    damage += unit.GetDamage(BattleController.instance.ai.GetUnit(), unit.ZOCModifer);
            }
        }
        return damage;
    }

    public static List<Tile> GetTotalRange(Unit unit)
    {
        List<Tile> mvtTiles = GetMovementTiles(unit);
        List<Tile> attackable = new List<Tile>();
        List<Tile> atkTiles;
        List<Tile> movementTiles = HexMap.GetMovementTiles(unit);
        int facing = unit.facing;
        Tile initial = unit.currentTile;
        foreach (Tile t in mvtTiles)
        {
            unit.currentTile = t;
            atkTiles = GetAttackTiles(unit);
            for (int i = 0; i < 6; i++)
            {
                unit.facing = i;
                atkTiles = HexMap.GetAttackTiles(unit);
                foreach (Tile tile in atkTiles)
                {
                    attackable.Add(tile);
                }
            }
        }
        unit.facing = facing;
        unit.currentTile = initial;
        return attackable;
    }

    public static void ShowAttackTiles(Unit unit) {
        ClearAttackTiles();
        foreach (Tile t in GetAttackTiles(unit)) {
            t.ShowAttackTile();
        }
    }

    public static void ClearAllTiles() {
        ClearMovementTiles();
        ClearAttackTiles();
    }

    public static void ClearAttackTiles() {
        while (showingAttackTiles.Count > 0) {
            showingAttackTiles.Pop().HideAttackTile();
        }
    }

    public static void ClearMovementTiles() {
        while (showingAttackOutlines.Count > 0)
        {
            GameObject.Destroy(showingAttackOutlines.Pop());
        }
        while (showingMovementTiles.Count > 0) {
            showingMovementTiles.Pop().HideMovementTile();
        }
        Object.Destroy(GameObject.Find("path"));
    }

    public static List<Tile> GetNeighbors(Tile tile) {
        List<Tile> neighbors = new List<Tile>();
        TileLocation position = tile.position;
        foreach (Vector2 dir in directions) {
            try {
                Tile neighbor = mapArray[position.row + (int)(dir.x)][position.col + (int)(dir.y)];
                neighbors.Add(neighbor);
            } catch {
                neighbors.Add(null);
            }
        }
        return neighbors;
    }

    public static bool AreNeighbors(Tile tile1, Tile tile2) {
        return GetNeighbors(tile1).Contains(tile2);
    }

    public static int Cost(Tile a, Tile b) {
        return (System.Math.Abs(-b.position.row+a.position.row-b.position.col+a.position.col)+System.Math.Abs(a.position.row- b.position.row)+System.Math.Abs(a.position.col - b.position.col))/2;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class HexMap : MonoBehaviour {
    private HexDimension hexDimension;
    public static List<List<Tile>> mapArray;
    public SelectionController selectionController; // ref to hack
    public static Stack<Tile> showingMovementTiles;
    public static Stack<Tile> showingAttackTiles;

    // Make a separate directions class/enum?
    public static readonly List<Vector2> directions = new List<Vector2> {
            new Vector2(0, 1), new Vector2(-1, 1), new Vector2(-1, 0), 
            new Vector2(0, -1), new Vector2(1, -1), new Vector2(1, 0)
        };

    void Awake() {
        hexDimension = this.gameObject.GetComponent<HexDimension>();
        mapArray = new List<List<Tile>>();
        showingAttackTiles = new Stack<Tile>();
        showingMovementTiles = new Stack<Tile>();
        ////// DEBUG CODE //////
        if (hexDimension == null)
        {
            Debug.Log("Error :: No Defined Hex Dimension for Hex Map - HexMap.cs");
        }
        this.gameObject.transform.position = new Vector3(hexDimension.globalTopLeftX, hexDimension.globalTopLeftY, 0); // temp
        ////////////////////////
    }

    // TODO :: think of a way to cache game objects later
    // Is this method even needed ??

    public void ClearMap() {
        Debug.Log("TO BE TESTED -> HexMap.cs");
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

    public static void ShowMovementTiles(Tile tile, int distance) {
        ClearMovementTiles();
        Queue<Tile> toCheck = new Queue<Tile>();
        Queue<int> dist = new Queue<int>();
        toCheck.Enqueue(tile);
        dist.Enqueue(distance);
        List<Tile> neighbors;
        while (toCheck.Count > 0) {
            Tile t = toCheck.Dequeue();
            distance = dist.Dequeue();
            if (distance > 0 && t.pathable ) {
                if (t.currentUnit == null || t.currentUnit.isPlayerUnit==SelectionController.selectedUnit.isPlayerUnit) {
                    t.ShowMovementTile();
                    neighbors = GetNeighbors(t);
                    foreach (Tile neighbor in neighbors) {
                        if (neighbor && !neighbor.MovementTileIsVisible()) {
                            toCheck.Enqueue(neighbor);
                            dist.Enqueue(distance - 1);
                        }
                    }
                }
            }
        }
    }

    public static List<Tile> GetAttackTiles(Tile tile) {
        List<Tile> output = new List<Tile>();
        Unit unit = tile.currentUnit;
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
                output.Add(mapArray[tile.position.row + (int)Vector2.Dot(pos,rowDot)][tile.position.col + (int)Vector2.Dot(pos, colDot)]);
            }
            catch { }
        }
        return output;
    }

    public static void ShowAttackTiles(Tile tile)
    {
        ClearAttackTiles();
        foreach (Tile t in GetAttackTiles(tile)) {
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
}

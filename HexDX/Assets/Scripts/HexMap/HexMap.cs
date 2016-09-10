using UnityEngine;
using System.Collections.Generic;

public class HexMap : MonoBehaviour {
    private HexDimension hexDimension;
    public static List<List<Tile>> mapArray;
    public SelectionController selectionController; // ref to hack

    // Make a separate directions class/enum?
    public static readonly List<Vector2> directions = new List<Vector2> {
            new Vector2(0, 1), new Vector2(-1, 0), new Vector2(-1, -1), 
            new Vector2(0, -1), new Vector2(1, 0), new Vector2(1, 1)
        };

    void Awake() {
        hexDimension = this.gameObject.GetComponent<HexDimension>();
        mapArray = new List<List<Tile>>();
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
            if (distance > 0 && t.pathable) {
                t.ShowMovementTile();                    
                neighbors = GetNeighbors(t);
                foreach (Tile neighbor in neighbors) {
                    if (neighbor && !neighbor.MovementTileIsVisible()){
                        toCheck.Enqueue(neighbor);
                        dist.Enqueue(distance - 1);
                    }
                }
            }
        }
    }

    public static void ClearMovementTiles() {
        foreach (List<Tile> row in mapArray) {
            foreach (Tile tile in row) {
                tile.HideMovementTile();
            }
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

    public static List<Tile> GetShortestPath(Tile a, Tile b) {
        int bound = Cost(a, b);
        List<Tile> shortestPath = new List<Tile>();
        while (true) {
            int t = Search(a,b, 0, bound, ref shortestPath);
            if (t == -1) {
                shortestPath.Add(a);
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

    private static int Search(Tile node, Tile dest, int g, int bound, ref List<Tile> currentPath) {
        int f = g + Cost(node, dest);
        if (f > bound) {
            return f;
        }
        if (node == dest) {
            return -1;
        }
        int min = int.MaxValue;
        foreach (Tile neighbor in GetNeighbors(node)) {
            if (neighbor!=null && neighbor.pathable) {
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
        
    private static int Cost(Tile a, Tile b) {
        return System.Math.Max(System.Math.Abs(a.position.row- b.position.row), System.Math.Abs(a.position.col - b.position.col))/2;
    }
}

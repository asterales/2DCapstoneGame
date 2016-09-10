using UnityEngine;
using System.Collections.Generic;

public class MovementTile : MonoBehaviour {

    public Tile tile;
    public static List<Tile> path;
   
    public void OnMouseDown() {
        if (SelectionController.selectedTile == tile) {
            path = new List<Tile>() { tile };
        } else {
            tile.OnMouseDown();
        }
    }

    public void OnMouseEnter() {
        if (Input.GetMouseButton(0) && path != null ) {
            if (path.Count>1 && tile == path[path.Count - 2])
            {
                path.Remove(path[path.Count - 1]);
            }
            else
            {
                if (path.Count <= SelectionController.selectedUnit.unitStats.mvtRange
                        && HexMap.AreNeighbors(tile, path[path.Count - 1]))
                {
                    path.Add(tile);
                }
                else
                {
                    path = new List<Tile>();
                    shortestPath(tile, SelectionController.selectedTile);
                }
            }
            drawPath();
        }
    }

    //refactor later
    public void drawPath() {
        PlayerBattleController pbc = GameObject.Find("TestHexMap").GetComponent<PlayerBattleController>();
        Object.Destroy(GameObject.Find("path"));
        if (path!=null && path.Count > 1) {
            GameObject pathDraw = new GameObject("path");
            Tile prev = path[0];
            int direction= 0;
            GameObject temp;
            for (int i = 1; i < path.Count; i++) {
                temp = new GameObject();
                temp.transform.parent = pathDraw.transform;
                SpriteRenderer circle = temp.AddComponent<SpriteRenderer>();
                circle.sortingOrder = 1;
                circle.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
                circle.sprite = pbc.circleSprite;
                circle.transform.position = prev.transform.position;
                direction = HexMap.GetNeighbors(prev).IndexOf(path[i]);
                temp = new GameObject();
                temp.transform.parent = pathDraw.transform;
                SpriteRenderer line = temp.AddComponent<SpriteRenderer>();
                line.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
                line.sortingOrder = 1;
                Debug.Log(direction);
                line.sprite = pbc.lineSprites[direction];
                line.transform.position = prev.transform.position;
                prev = path[i];
            }
            direction = HexMap.GetNeighbors(path[path.Count - 2]).IndexOf(prev);
            temp = new GameObject();
            temp.transform.parent = pathDraw.transform;
            SpriteRenderer arrow = temp.AddComponent<SpriteRenderer>();
            arrow.sprite = pbc.arrowSprites[direction];
            arrow.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
            arrow.sortingOrder = 1;
            arrow.transform.position = prev.transform.position;
        }
    }


    public bool shortestPath(Tile a, Tile b) {
        int bound = cost(a, b);
        while (true) {
            int t = search(a,b, 0, bound);
            if (t == -1) {
                path.Add(a);
                return true;
            }
            if (t == int.MaxValue) {
                return false;
            }
            bound = t;
            path = new List<Tile>();
        }
    }

    private int search(Tile node, Tile dest, int g, int bound) {
        int f = g + cost(node, dest);
        if (f > bound) {
            return f;
        }
        if (node == dest) {
            return -1;
        }
        int min = int.MaxValue;
        foreach (Tile neighbor in HexMap.GetNeighbors(node)) {
            if (neighbor!=null && neighbor.pathable) {
                int t = search(neighbor, dest, g + 1, bound);
                if (t == -1) {
                    path.Add(neighbor);
                    return -1;
                }
                if (t < min) {
                    min = t;
                }
            }

        }
        return min;

    }
        
    private int cost(Tile a, Tile b) {
        return System.Math.Max(System.Math.Abs(a.position.row- b.position.row), System.Math.Abs(a.position.col - b.position.col))/2;
    }
    // Update is called once per frame
    void Update () {
	
	}
}

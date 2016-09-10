using UnityEngine;
using System.Collections.Generic;

public class MovementTile : MonoBehaviour {

    public Tile tile;
    private SelectionController sc;
    public static List<Tile> path;
   
    // Use this for initialization
    void Start () {
        sc = GameObject.Find("TestHexMap").GetComponent<HexMap>().GetComponent<SelectionController>();
    }

    public void OnMouseDown()
    {
        if (SelectionController.selectedTile.Equals(tile))
        {
            path = new List<Tile>();
            path.Add(tile);
        }
        //if (sc.selectedTile.currentUnit)
        //{
        //    Unit unit = sc.selectedTile.currentUnit;
        //    sc.selectedTile.currentUnit = null;
        //    unit.Move(tile);
        //    tile.currentUnit = unit;
        //    HexMap.ClearMovementTiles();
        //    sc.ClearSelection();
        //}
    }

    public void OnMouseEnter()
    {
        if (Input.GetMouseButton(0) && path != null)
            if ( path.Count<=SelectionController.selectedUnit.unitStats.speed && path[path.Count-1].GetNeighbors().IndexOf(tile)!=-1)
            {
                path.Add(tile);
                drawPath();

            }
            else
            {
                path = new List<Tile>();
                shortestPath(tile, SelectionController.selectedTile);
                drawPath();
            }
    }

    public void drawPath()
    {
        if (path!=null && path.Count > 1) {
            PlayerBattleController pbc = GameObject.Find("TestHexMap").GetComponent<PlayerBattleController>();
            Object.Destroy(GameObject.Find("path"));
            GameObject pathDraw = new GameObject("path");
            Tile prev = path[0];
            int direction= 0;
            GameObject temp;
            for (int i = 1; i < path.Count; i++)
            {
                temp = new GameObject();
                temp.transform.parent = pathDraw.transform;
                SpriteRenderer circle = temp.AddComponent<SpriteRenderer>();
                circle.sortingOrder = 1;
                circle.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
                circle.sprite = pbc.circleSprite;
                circle.transform.position = prev.transform.position;
                direction = prev.GetNeighbors().IndexOf(path[i]);
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
            direction = path[path.Count - 2].GetNeighbors().IndexOf(prev);
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
        while (true)
        {
            int t = search(a,b, 0, bound);
            if (t == -1)
            {
                path.Add(a);
                return true;
            }
            if (t == int.MaxValue)
                return false;
            bound = t;
            path = new List<Tile>();
        }
    }

    private int search(Tile node, Tile dest, int g, int bound)
    {
        int f = g + cost(node, dest);
        if (f > bound)
            return f;
        if (node == dest)
            return -1;
        int min = int.MaxValue;
        foreach (Tile neighbor in node.GetNeighbors())
        {
            if (neighbor!=null && neighbor.pathable)
            {
                int t = search(neighbor, dest, g + 1, bound);
                if (t == -1)
                {
                    path.Add(neighbor);
                    return -1;
                }
                if (t < min)
                    min = t;
            }

        }
        return min;

    }
        
    private int cost(Tile a, Tile b)
    {
        return System.Math.Max(System.Math.Abs(a.position.row- b.position.row), System.Math.Abs(a.position.col - b.position.col))/2;
    }
    // Update is called once per frame
    void Update () {
	
	}
}

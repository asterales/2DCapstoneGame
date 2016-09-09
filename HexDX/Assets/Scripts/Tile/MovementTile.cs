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
        if (Input.GetMouseButton(0) && path!=null && path.Count<=SelectionController.selectedUnit.unitStats.speed)
        {
            path.Add(tile);
        }
        drawPath();
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

    // Update is called once per frame
    void Update () {
	
	}
}

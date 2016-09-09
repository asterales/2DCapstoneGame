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
    }

    // Update is called once per frame
    void Update () {
	
	}
}

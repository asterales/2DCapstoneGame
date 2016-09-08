using UnityEngine;
using System.Collections.Generic;

public class MovementTile : MonoBehaviour {

    public Tile tile;
    private SelectionController sc;
    public static List<Vector3> path;
    // Use this for initialization
    void Start () {
        sc = GameObject.Find("TestHexMap").GetComponent<HexMap>().GetComponent<SelectionController>();
    }

    public void OnMouseDown()
    {
        if (SelectionController.selectedTile.Equals(tile))
        {
            path = new List<Vector3>();
            path.Add(tile.transform.position - new Vector3(0, 0, 1.0f));
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
        if (Input.GetMouseButton(0) && path!=null)
        {
            path.Add(tile.transform.position - new Vector3(0, 0, 1.0f));
            SelectionController.selectedUnit.SetTile(tile);
            Debug.Log(path.Count);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}

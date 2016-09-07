using UnityEngine;
using System.Collections;

public class MovementTile : MonoBehaviour {

    public Tile tile;
    private SelectionController sc;
    // Use this for initialization
    void Start () {
         sc = GameObject.Find("TestHexMap").GetComponent<HexMap>().GetComponent<SelectionController>();
    }

    public void OnMouseDown()
    {
        if (sc.selectedTile.currentUnit)
        {
            Unit unit = sc.selectedTile.currentUnit;
            sc.selectedTile.currentUnit = null;
            unit.Move(tile);
            tile.currentUnit = unit;
            HexMap.ClearMovementTiles();
            sc.ClearSelection();
        }
    }
    // Update is called once per frame
    void Update () {
	
	}
}

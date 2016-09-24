using UnityEngine;
using System.Collections.Generic;

public class RowOfTiles : MonoBehaviour {
    public List<Tile> tiles;

	void Start () {
	    ////// DEBUG CODE //////
        if (tiles.Count == 0)
        {
            Debug.Log("ERROR :: Number of Columns needs to be more than zero -> RowOfTiles.cs");
        }
        ////////////////////////
	}
}

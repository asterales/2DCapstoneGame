using UnityEngine;
using System.Collections.Generic;

public class RowContainer : MonoBehaviour {
    public List<RowOfTiles> tileRows;

	void Start () {
        ////// DEBUG CODE //////
	    if (tileRows.Count == 0)
        {
            Debug.Log("Error :: Need More than 0 rows of tiles -> RowContainer");
        }
        ////////////////////////
	}
}

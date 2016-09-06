using UnityEngine;
using System.Collections.Generic;

public class HexMap : MonoBehaviour {
    private HexDimension hexDimension;
    public List<List<Tile>> mapArray;
    public SelectionController selectionController; // ref to hack

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

    public void ClearMap()
    {
        Debug.Log("TO BE TESTED -> HexMap.cs");
        // remove and delete all current tiles
        for (int i = 0; i < mapArray.Count; i++)
        {
            while (mapArray[i].Count > 0)
            {
                int lastIndex = mapArray[i].Count - 1;
                Tile tile = mapArray[i][lastIndex];
                mapArray[i].RemoveAt(lastIndex);
                // maybe do more cleanup or caching here
                Destroy(tile);
            }
        }

        // delete all lists in mapArray
        while (mapArray.Count > 0)
        {
            int lastIndex = mapArray.Count - 1;
            mapArray.RemoveAt(lastIndex);
        }
    }
}

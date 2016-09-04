using UnityEngine;
using System.Collections.Generic;

public class HexMap : MonoBehaviour {
    private HexDimension hexDimension;
    public List<List<Tile>> mapArray;
    
    void Start () {
        hexDimension = this.gameObject.GetComponent<HexDimension>();
        ////// DEBUG CODE //////
        if (hexDimension == null)
        {
            Debug.Log("Error :: No Defined Hex Dimension for Hex Map - HexMap.cs");
        }
        ////////////////////////
    }

    public void SetMap(List<List<Tile>> map){
        mapArray = map;
    }
}

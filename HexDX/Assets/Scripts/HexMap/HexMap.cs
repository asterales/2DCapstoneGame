using UnityEngine;
using System.Collections.Generic;

public class HexMap : MonoBehaviour {
    public List<List<Tile>> mapArray;

    public void SetMap(List<List<Tile>> map){
        mapArray = map;
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

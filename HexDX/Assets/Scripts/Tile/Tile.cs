using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour{
    protected int type;
    protected Sprite sprite;
    public bool isPathable;

    public void MakeTile(int type, Sprite sprite)
    {
        this.type = type;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

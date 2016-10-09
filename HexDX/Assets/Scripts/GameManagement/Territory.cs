using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Territory : MonoBehaviour {
    public List<Territory> neighbors;
    public LevelManager lm;
    public bool captured;

    public void Start()
    {
        this.GetComponent<SpriteRenderer>().color = Color.gray;
    }
    public void OnMouseEnter()
    {
        this.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void OnMouseExit()
    {
        this.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    public void OnMouseDown()
    {
        lm.StartLevel();
    }
}

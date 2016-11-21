using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mob : MonoBehaviour {
    public List<Unit> members;
	// Use this for initialization
	void Start () {
        members = new List<Unit>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addMember(Unit unit)
    {
        members.Add(unit);
    }

    public virtual bool triggered()
    {
        return true;
    }
}

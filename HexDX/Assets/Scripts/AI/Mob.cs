using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mob {
    public List<Unit> members;
	// Use this for initialization
	void Start () {
        members = new List<Unit>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static Mob GetMob(int type)
    {
        switch (type)
        {
            case 0: return new OffensiveMob();
            case 1: return new DefensiveMob();
            case 2: return new SuperDefensiveMob();
        }
        return new OffensiveMob();
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SuperDefensiveMob : Mob {
    public SuperDefensiveMob()
    {
        members = new List<Unit>();
    }
    public override bool triggered()
    {
        foreach (Unit member in members)
        {
            if (member.Health < member.MaxHealth)
                return true;
        }
        return false;
    }
}

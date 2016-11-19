using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SuperDefenseiveMob : Mob {

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

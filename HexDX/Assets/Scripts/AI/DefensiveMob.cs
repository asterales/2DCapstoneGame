﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefensiveMob : Mob {
    public DefensiveMob()
    {
        members = new List<Unit>();
    }
    public override bool triggered()
    {
        foreach (Unit member in members)
        {
            foreach (Unit unit in BattleController.instance.player.units)
            {
                if (member.enabled && unit.enabled && member.HasInTotalRange(unit))
                {
                    return true;
                }
            }
        }
        return false;
    }
}

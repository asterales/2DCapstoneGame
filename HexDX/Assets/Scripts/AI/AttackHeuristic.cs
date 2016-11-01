using UnityEngine;

public class AttackHeuristic : Heuristic {
    public Unit unitToBeAttacked;
    public int direction;
    public bool attackingFlank;           // if attacking flank
    public bool attackingDirect;          // if attacking directly
    public bool attackingSneak;           // if attacking sneak
    public bool attackKills;              // if the attack will kill
    public float stateComparison;         // comparing the unit stats (0.0 <= v <= 1.0)

    public AttackHeuristic(Tile t, Unit u)
    {
        unitToBeAttacked = null;
        unit = u;
        tile = t;
    }

    public AttackHeuristic(Unit attacker, Unit victim, Tile location, int dir)
    {
        unit = attacker;
        unitToBeAttacked = victim;
        tile = location;
        direction = dir;
    }

    public override void EvaluateData()
    {
        DecideAttackDirectionBonus();
        DecideAttackStates();
    }

    private void DecideAttackDirectionBonus()
    {
        attackingFlank = Mathf.Abs(unitToBeAttacked.facing - direction) == 1 || Mathf.Abs(unitToBeAttacked.facing - direction) == 5;
        attackingSneak = unitToBeAttacked.facing == direction;
        attackingDirect = !attackingFlank && !attackingSneak;
    }

    private void DecideAttackStates()
    {
        // state comparison right now only compares health and damage done
        // calculate damage and check if kills
        float modifier = 1.0f;
        if (attackingFlank) modifier = 2.0f;
        if (attackingFlank) modifier = 1.5f;
        float damage = (int)(unit.Attack * (50.0f / (50.0f + (float)unitToBeAttacked.Defense)));
        damage *= modifier;
        if (damage > unitToBeAttacked.Health) attackKills = true;

        // Calculate State Comparison ::
        // 0.5 = health comparison
        // 0.5 = damage comparison assuming direct * 0.8

        float healthComparison = 0.0f;
        float damageComparison = 0.0f;

        float enemyDamage = (int)(unitToBeAttacked.Attack * (50.0f / (50.0f + (float)unit.Defense))) * 0.8f;

        healthComparison = (unit.Health - unitToBeAttacked.Health) / unit.Health * 0.5f;
        if (attackingFlank || attackingSneak)
        {
            damageComparison = 1.0f;
        }
        else
        {
            damageComparison = (damage - enemyDamage) / damage * 0.5f;
        }

        stateComparison = healthComparison + damageComparison;
    }

    public override float CalculateHeuristic(AIWeights weights)
    {
        float heuristic = 0.0f;
        if (attackingDirect) heuristic += weights.unitAttackAdvantage;
        if (attackingFlank) heuristic += weights.unitFlankAdvantage;
        if (attackingSneak) heuristic += weights.unitSneakAdvangage;
        if (attackKills) heuristic += weights.unitKillAdvantage;
        heuristic += stateComparison * weights.unitStateComparison;
        heuristic *= weights.unitGlobal;
        //Debug.Log("ATTACK HEURISTIC: " + heuristic);
        return heuristic;
    }
}

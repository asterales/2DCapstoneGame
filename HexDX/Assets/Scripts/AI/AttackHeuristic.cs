public class AttackHeuristic : Heuristic {
    public Unit unitToBeAttacked;
    bool attackingFlank;           // if attacking flank
    bool attackingDirect;          // if attacking directly
    bool attackingSneak;           // if attacking sneak
    bool attackKills;              // if the attack will kill
    float stateComparison;         // comparing the unit stats (0.0 <= v <= 1.0)

    public AttackHeuristic(Tile t, Unit u)
    {
        unitToBeAttacked = null;
        unit = u;
        tile = t;
    }

    public AttackHeuristic(Unit attacker, Unit victim, Tile location)
    {
        unit = attacker;
        unitToBeAttacked = victim;
        tile = location;
    }

    public override void EvaluateData()
    {
        DecideAttackDirectionBonus();
        DecideAttackStates();
    }

    private void DecideAttackDirectionBonus()
    {
        // check if sneak
        // check if flank
        // check if hit
        // to be implemented
        attackingDirect = false;
        attackingFlank = false;
        attackingSneak = false;
    }

    private void DecideAttackStates()
    {
        // check if kills
        // check if lower stats
        // check if does more damage
        // to be implemented
        stateComparison = 0.0f;
        attackKills = false;
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
        return heuristic;
    }
}

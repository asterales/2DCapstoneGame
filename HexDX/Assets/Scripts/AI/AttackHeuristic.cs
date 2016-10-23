public class AttackHeuristic : Heuristic {
    public Unit unitToBeAttacked;
    public Unit unitAttacking;
    public Tile attackLocation;    // where the attacking unit will be
    bool attackingFlank;           // if attacking flank
    bool attackingDirect;          // if attacking directly
    bool attackingSneak;           // if attacking sneak
    bool attackKills;              // if the attack will kill
    float stateComparison;         // comparing the unit stats (0.0 <= v <= 1.0)

    public AttackHeuristic()
    {
        unitToBeAttacked = null;
        unitAttacking = null;
        attackLocation = null;
    }

    public AttackHeuristic(Unit attacker, Unit victim, Tile location)
    {
        unitAttacking = attacker;
        unitToBeAttacked = victim;
        attackLocation = location;
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

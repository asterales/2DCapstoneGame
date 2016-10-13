// A container for the weights of an AI

public class AIWeights {
    // tile weights
    public float tileStatBoost;
    public float tileEnemyCloosness;
    public float tileEnemyDistance;
    public float tileClosenessObjective;
    public float tileGlobal;
    // facing weights
    public float faceFlankDisadvantage;
    public float faceSneakDisadvantage;
    public float faceAttackDisadvantage;
    public float faceStateComparison;
    public float faceDeathDisadvantage;
    public float faceGlobal;
    // attack weights
    public float unitFlankAdvantage;
    public float unitSneakAdvangage;
    public float unitAttackAdvantage;
    public float unitKillAdvantage;
    public float unitStateComparison;
    public float unitGlobal;

    public AIWeights()
    {
        tileStatBoost = 0.0f;
        tileEnemyCloosness = 0.0f;
        tileEnemyDistance = 0.0f;
        tileClosenessObjective = 0.0f;
        tileGlobal = 0.0f;
        faceFlankDisadvantage = 0.0f;
        faceSneakDisadvantage = 0.0f;
        faceAttackDisadvantage = 0.0f;
        faceStateComparison = 0.0f;
        faceDeathDisadvantage = 0.0f;
        faceGlobal = 0.0f;
        unitFlankAdvantage = 0.0f;
        unitSneakAdvangage = 0.0f;
        unitAttackAdvantage = 0.0f;
        unitKillAdvantage = 0.0f;
        unitStateComparison = 0.0f;
        unitGlobal = 0.0f;
    }
}

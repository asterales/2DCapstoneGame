using UnityEngine;

public class LEAI : MonoBehaviour {
    // tile weights
    public int tileStatBoost;
    public int tileEnemyCloseness;
    public int tileEnemyDistance;
    public int tileClosenessObjective;
    public int tileRefrain;
    public int tileGlobal;
    // facing weights
    public int faceFlankDisadvantage;
    public int faceSneakDisadvantage;
    public int faceAttackDisadvantage;
    public int faceStateComparison;
    public int faceDeathDisadvantage;
    public int faceClosestEnemy;
    public int faceClosestObjective;
    public int faceGlobal;
    // attack weights
    public int unitFlankAdvantage;
    public int unitSneakAdvangage;
    public int unitAttackAdvantage;
    public int unitKillAdvantage;
    public int unitStateComparison;
    public int unitFaceAttackingDirectly;
    public int unitGlobal;

    public string id;
    public int index;

    void Start()
    {
        id = "defaultID";
        tileStatBoost = 0;
        tileEnemyCloseness = 0;
        tileEnemyDistance = 0;
        tileClosenessObjective = 0;
        tileRefrain = 0;
        tileGlobal = 0;
        faceFlankDisadvantage = 0;
        faceSneakDisadvantage = 0;
        faceAttackDisadvantage = 0;
        faceStateComparison = 0;
        faceDeathDisadvantage = 0;
        faceClosestEnemy = 0;
        faceClosestObjective = 0;
        faceGlobal = 0;
        unitFlankAdvantage = 0;
        unitSneakAdvangage = 0;
        unitAttackAdvantage = 0;
        unitKillAdvantage = 0;
        unitStateComparison = 0;
        unitFaceAttackingDirectly = 0;
        unitGlobal = 0;
        index = 0;
    }

    public void ParseAI(string fileName, string data)
    {
        // to be implemented
    }

    public void WriteAI()
    {
        // to be implemented
    }
}

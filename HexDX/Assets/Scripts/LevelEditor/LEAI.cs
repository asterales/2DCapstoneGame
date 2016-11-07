using UnityEngine;

public class LEAI : MonoBehaviour {
    // tile weights
    public float tileStatBoost;
    public float tileEnemyCloseness;
    public float tileEnemyDistance;
    public float tileClosenessObjective;
    public float tileRefrain;
    public float tileGlobal;
    // facing weights
    public float faceFlankDisadvantage;
    public float faceSneakDisadvantage;
    public float faceAttackDisadvantage;
    public float faceStateComparison;
    public float faceDeathDisadvantage;
    public float faceClosestEnemy;
    public float faceClosestObjective;
    public float faceGlobal;
    // attack weights
    public float unitFlankAdvantage;
    public float unitSneakAdvangage;
    public float unitAttackAdvantage;
    public float unitKillAdvantage;
    public float unitStateComparison;
    public float unitFaceAttackingDirectly;
    public float unitGlobal;

    public string id;

    void Start()
    {
        id = "defaultID";
        tileStatBoost = 0.0f;
        tileEnemyCloseness = 0.0f;
        tileEnemyDistance = 0.0f;
        tileClosenessObjective = 0.0f;
        tileRefrain = 0.0f;
        tileGlobal = 0.0f;
        faceFlankDisadvantage = 0.0f;
        faceSneakDisadvantage = 0.0f;
        faceAttackDisadvantage = 0.0f;
        faceStateComparison = 0.0f;
        faceDeathDisadvantage = 0.0f;
        faceClosestEnemy = 0.0f;
        faceClosestObjective = 0.0f;
        faceGlobal = 0.0f;
        unitFlankAdvantage = 0.0f;
        unitSneakAdvangage = 0.0f;
        unitAttackAdvantage = 0.0f;
        unitKillAdvantage = 0.0f;
        unitStateComparison = 0.0f;
        unitFaceAttackingDirectly = 0.0f;
        unitGlobal = 0.0f;
    }
}

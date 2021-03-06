﻿using System;

// A container for the weights of an AI

public class AIWeights {
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
    // input data
    public string fileName;

    public AIWeights()
    {
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
        fileName = "";
        //initializeAttackEasy();
        //initializeDefenseEasy();
    }

    public void ReadFromFile(string name)
    {
        fileName = name;
        string[] aiRows = GameResources.GetFileLines(fileName);
        tileStatBoost = (float)Convert.ToDouble(aiRows[0].Trim());
        tileEnemyCloseness = (float)Convert.ToDouble(aiRows[1].Trim());
        tileEnemyDistance = (float)Convert.ToDouble(aiRows[2].Trim());
        tileClosenessObjective = (float)Convert.ToDouble(aiRows[3].Trim());
        tileRefrain = (float)Convert.ToDouble(aiRows[4].Trim());
        tileGlobal = (float)Convert.ToDouble(aiRows[5].Trim());
        faceFlankDisadvantage = (float)Convert.ToDouble(aiRows[6].Trim());
        faceSneakDisadvantage = (float)Convert.ToDouble(aiRows[7].Trim());
        faceAttackDisadvantage = (float)Convert.ToDouble(aiRows[8].Trim());
        faceStateComparison = (float)Convert.ToDouble(aiRows[9].Trim());
        faceDeathDisadvantage = (float)Convert.ToDouble(aiRows[10].Trim());
        faceClosestEnemy = (float)Convert.ToDouble(aiRows[11].Trim());
        faceClosestObjective = (float)Convert.ToDouble(aiRows[12].Trim());
        faceGlobal = (float)Convert.ToDouble(aiRows[13].Trim());
        unitFlankAdvantage = (float)Convert.ToDouble(aiRows[14].Trim());
        unitSneakAdvangage = (float)Convert.ToDouble(aiRows[15].Trim());
        unitAttackAdvantage = (float)Convert.ToDouble(aiRows[16].Trim());
        unitKillAdvantage = (float)Convert.ToDouble(aiRows[17].Trim());
        unitStateComparison = (float)Convert.ToDouble(aiRows[18].Trim());
        unitFaceAttackingDirectly = (float)Convert.ToDouble(aiRows[19].Trim());
        unitGlobal = (float)Convert.ToDouble(aiRows[20].Trim());
    }

    public string WriteToString()
    {
        string data = "";
        data += (double)tileStatBoost + "\n";
        data += (double)tileEnemyCloseness + "\n";
        data += (double)tileEnemyDistance + "\n";
        data += (double)tileClosenessObjective + "\n";
        data += (double)tileRefrain + "\n";
        data += (double)tileGlobal + "\n";
        data += (double)faceFlankDisadvantage + "\n";
        data += (double)faceSneakDisadvantage + "\n";
        data += (double)faceAttackDisadvantage + "\n";
        data += (double)faceStateComparison + "\n";
        data += (double)faceDeathDisadvantage + "\n";
        data += (double)faceClosestEnemy + "\n";
        data += (double)faceClosestObjective + "\n";
        data += (double)faceGlobal + "\n";
        data += (double)unitFlankAdvantage + "\n";
        data += (double)unitSneakAdvangage + "\n";
        data += (double)unitAttackAdvantage + "\n";
        data += (double)unitKillAdvantage + "\n";
        data += (double)unitStateComparison + "\n";
        data += (double)unitFaceAttackingDirectly + "\n";
        data += (double)unitGlobal + "\n";
        return data;
    }

    // TODO :: IMPLEMENT THIS IN A FILE SOMEWHERE
    public void initializeAttackEasy()
    {
        tileStatBoost = 0.0f;
        tileEnemyCloseness = 1.0f;
        tileEnemyDistance = 0.0f;
        tileClosenessObjective = 0.0f;
        tileRefrain = 0.0f;
        tileGlobal = 1.0f;
        faceFlankDisadvantage = 0.0f;
        faceSneakDisadvantage = 0.0f;
        faceAttackDisadvantage = 0.0f;
        faceStateComparison = 0.0f;
        faceDeathDisadvantage = 0.0f;
        faceClosestEnemy = 1.0f;
        faceClosestObjective = 0.0f;
        faceGlobal = 1.0f;
        unitFlankAdvantage = 0.0f;
        unitSneakAdvangage = 0.0f;
        unitAttackAdvantage = 1.0f;
        unitKillAdvantage = 0.0f;
        unitStateComparison = 0.0f;
        unitFaceAttackingDirectly = 0.2f;
        unitGlobal = 1.0f;
    }

    public void initializeDefenseEasy()
    {
        tileStatBoost = 0.0f;
        tileEnemyCloseness = 0.0f;
        tileEnemyDistance = 0.0f;
        tileClosenessObjective = 0.0f;
        tileRefrain = 0.5f;
        tileGlobal = 1.0f;
        faceFlankDisadvantage = 0.0f;
        faceSneakDisadvantage = 0.0f;
        faceAttackDisadvantage = 0.0f;
        faceStateComparison = 0.0f;
        faceDeathDisadvantage = 0.0f;
        faceClosestEnemy = 1.0f;
        faceClosestObjective = 0.0f;
        faceGlobal = 1.0f;
        unitFlankAdvantage = 1.0f;
        unitSneakAdvangage = 0.0f;
        unitAttackAdvantage = 1.0f;
        unitKillAdvantage = 0.0f;
        unitStateComparison = 0.0f;
        unitFaceAttackingDirectly = 1.0f;
        unitGlobal = 1.0f;
    }
}

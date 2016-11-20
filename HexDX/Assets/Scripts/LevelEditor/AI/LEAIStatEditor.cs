using UnityEngine;
using UnityEngine.UI;

public class LEAIStatEditor : MonoBehaviour {
    public LEAIStatTable reference;
    public LEAI currentAI;
    public LEIncrementButton singleIncrement;
    public LEIncrementButton doubleIncrement;
    public LEIncrementButton singleDecrement;
    public LEIncrementButton doubleDecrement;
    public Text text;
    public LEAIStatID statType;
    public string statName;
    public int singleValue = 1;
    public int doubleValue = 10;

    void Start() {
        currentAI = null;

        ////// DEBUG CODE //////
        if (reference == null)
        {
            Debug.Log("ERROR :: Reference to Stat Table needs to be defined -> LEAIStatEditor.cs");
        }
        if (singleIncrement == null)
        {
            Debug.Log("ERROR :: Reference to SingleIncrement Button needs to be defined -> LEAIStatEditor.cs");
        }
        if (doubleIncrement == null)
        {
            Debug.Log("ERROR :: Reference to DoubleIncrement Button needs to be defined -> LEAIStatEditor.cs");
        }
        if (singleDecrement == null)
        {
            Debug.Log("ERROR :: Reference to SingleDecrement Button needs to be defined -> LEAIStatEditor.cs");
        }
        if (doubleDecrement == null)
        {
            Debug.Log("ERROR :: Reference to DoubleDecrement Button needs to be defined -> LEAIStatEditor.cs");
        }
        ////////////////////////
	}

    public void TurnOn()
    {
        if (singleIncrement) singleIncrement.TurnOn();
        if (doubleIncrement) doubleIncrement.TurnOn();
        if (singleDecrement) singleDecrement.TurnOn();
        if (doubleDecrement) doubleDecrement.TurnOn();
    }

    public void TurnOff()
    {
        if (singleIncrement) singleIncrement.TurnOff();
        if (doubleIncrement) doubleIncrement.TurnOff();
        if (singleDecrement) singleDecrement.TurnOff();
        if (doubleDecrement) doubleDecrement.TurnOff();
    }

    public void ChangeState(int val)
    {
        switch (statType)
        {
            case LEAIStatID.TILESTATBOOST:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.tileStatBoost + val < 0) currentAI.tileStatBoost = 0;
                        else if (currentAI.tileStatBoost + val > 100) currentAI.tileStatBoost = 100;
                        else currentAI.tileStatBoost += val;
                    }
                    break;
                }
            case LEAIStatID.TILEENEMYCLOSENESS:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.tileEnemyCloseness + val < 0) currentAI.tileEnemyCloseness = 0;
                        else if (currentAI.tileEnemyCloseness + val > 100) currentAI.tileEnemyCloseness = 100;
                        else currentAI.tileEnemyCloseness += val;
                    }
                    break;
                }
            case LEAIStatID.TILEENEMYDISTANCE:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.tileEnemyDistance + val < 0) currentAI.tileEnemyDistance = 0;
                        else if (currentAI.tileEnemyDistance + val > 100) currentAI.tileEnemyDistance = 100;
                        else currentAI.tileEnemyDistance += val;
                    }
                    break;
                }
            case LEAIStatID.TILECLOSENESSOBJECTIVE:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.tileClosenessObjective + val < 0) currentAI.tileClosenessObjective = 0;
                        else if (currentAI.tileClosenessObjective + val > 100) currentAI.tileClosenessObjective = 100;
                        else currentAI.tileClosenessObjective += val;
                    }
                    break;
                }
            case LEAIStatID.TILEREFRAIN:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.tileRefrain + val < 0) currentAI.tileRefrain = 0;
                        else if (currentAI.tileRefrain + val > 100) currentAI.tileRefrain = 100;
                        else currentAI.tileRefrain += val;
                    }
                    break;
                }
            case LEAIStatID.TILEGLOBAL:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.tileGlobal + val < 0) currentAI.tileGlobal = 0;
                        else if (currentAI.tileGlobal + val > 100) currentAI.tileGlobal = 100;
                        else currentAI.tileGlobal += val;
                    }
                    break;
                }
            case LEAIStatID.FACEFLANKDISADVANTAGE:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.faceFlankDisadvantage + val < 0) currentAI.faceFlankDisadvantage = 0;
                        else if (currentAI.faceFlankDisadvantage + val > 100) currentAI.faceFlankDisadvantage = 100;
                        else currentAI.faceFlankDisadvantage += val;
                    }
                    break;
                }
            case LEAIStatID.FACESNEAKDISADVANTAGE:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.faceSneakDisadvantage + val < 0) currentAI.faceSneakDisadvantage = 0;
                        else if (currentAI.faceSneakDisadvantage + val > 100) currentAI.faceSneakDisadvantage = 100;
                        else currentAI.faceSneakDisadvantage += val;
                    }
                    break;
                }
            case LEAIStatID.FACEATTACKDISADVANTAGE:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.faceAttackDisadvantage + val < 0) currentAI.faceAttackDisadvantage = 0;
                        else if (currentAI.faceAttackDisadvantage + val > 100) currentAI.faceAttackDisadvantage = 100;
                        else currentAI.faceAttackDisadvantage += val;
                    }
                    break;
                }
            case LEAIStatID.FACESTATECOMPARISON:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.faceStateComparison + val < 0) currentAI.faceStateComparison = 0;
                        else if (currentAI.faceStateComparison + val > 100) currentAI.faceStateComparison = 100;
                        else currentAI.faceStateComparison += val;
                    }
                    break;
                }
            case LEAIStatID.FACEDEATHDISADVANTAGE:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.faceDeathDisadvantage + val < 0) currentAI.faceDeathDisadvantage = 0;
                        else if (currentAI.faceDeathDisadvantage + val > 100) currentAI.faceDeathDisadvantage = 100;
                        else currentAI.faceDeathDisadvantage += val;
                    }
                    break;
                }
            case LEAIStatID.FACECLOSESTENEMY:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.faceClosestEnemy + val < 0) currentAI.faceClosestEnemy = 0;
                        else if (currentAI.faceClosestEnemy + val > 100) currentAI.faceClosestEnemy = 100;
                        else currentAI.faceClosestEnemy += val;
                    }
                    break;
                }
            case LEAIStatID.FACECLOSESTOBJECTIVE:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.faceClosestObjective + val < 0) currentAI.faceClosestObjective = 0;
                        else if (currentAI.faceClosestObjective + val > 100) currentAI.faceClosestObjective = 100;
                        else currentAI.faceClosestObjective += val;
                    }
                    break;
                }
            case LEAIStatID.FACEGLOBAL:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.faceGlobal + val < 0) currentAI.faceGlobal = 0;
                        else if (currentAI.faceGlobal + val > 100) currentAI.faceGlobal = 100;
                        else currentAI.faceGlobal += val;
                    }
                    break;
                }
            case LEAIStatID.UNITFLANKADVANTAGE:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.unitFlankAdvantage + val < 0) currentAI.unitFlankAdvantage = 0;
                        else if (currentAI.unitFlankAdvantage + val > 100) currentAI.unitFlankAdvantage = 100;
                        else currentAI.unitFlankAdvantage += val;
                    }
                    break;
                }
            case LEAIStatID.UNITSNEAKADVANTAGE:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.unitSneakAdvangage + val < 0) currentAI.unitSneakAdvangage = 0;
                        else if (currentAI.unitSneakAdvangage + val > 100) currentAI.unitSneakAdvangage = 100;
                        else currentAI.unitSneakAdvangage += val;
                    }
                    break;
                }
            case LEAIStatID.UNITATTACKADVANTAGE:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.unitAttackAdvantage + val < 0) currentAI.unitAttackAdvantage = 0;
                        else if (currentAI.unitAttackAdvantage + val > 100) currentAI.unitAttackAdvantage = 100;
                        else currentAI.unitAttackAdvantage += val;
                    }
                    break;
                }
            case LEAIStatID.UNITKILLADVANTAGE:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.unitKillAdvantage + val < 0) currentAI.unitKillAdvantage = 0;
                        else if (currentAI.unitKillAdvantage + val > 100) currentAI.unitKillAdvantage = 100;
                        else currentAI.unitKillAdvantage += val;
                    }
                    break;
                }
            case LEAIStatID.UNITSTATECOMPARISON:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.unitStateComparison + val < 0) currentAI.unitStateComparison = 0;
                        else if (currentAI.unitStateComparison + val > 100) currentAI.unitStateComparison = 100;
                        else currentAI.unitStateComparison += val;
                    }
                    break;
                }
            case LEAIStatID.UNITFACEATTACKINGDIRECTLY:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.unitFaceAttackingDirectly + val < 0) currentAI.unitFaceAttackingDirectly = 0;
                        else if (currentAI.unitFaceAttackingDirectly + val > 100) currentAI.unitFaceAttackingDirectly = 100;
                        else currentAI.unitFaceAttackingDirectly += val;
                    }
                    break;
                }
            case LEAIStatID.UNITGLOBAL:
                {
                    if (currentAI != null)
                    {
                        if (currentAI.unitGlobal + val < 0) currentAI.unitGlobal = 0;
                        else if (currentAI.unitGlobal + val > 100) currentAI.unitGlobal = 100;
                        else currentAI.unitGlobal += val;
                    }
                    break;
                }
            case LEAIStatID.FILENAME:
                {
                    if (currentAI != null)
                    {
                        if (val > 0) reference.NextAI();
                        else reference.PrevAI();
                    }
                    break;
                }
            default: break;
        }
    }
}

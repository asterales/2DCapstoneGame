using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LEStatEditor : MonoBehaviour {
    public LEUnitSettings currentSettings;
    public LEUnitInstance currentInstance;
    public LEIncrementButton singleIncrement;
    public LEIncrementButton doubleIncrement;
    public LEIncrementButton tripleIncrement;
    public LEIncrementButton singleDecrement;
    public LEIncrementButton doubleDecrement;
    public LEIncrementButton tripleDecrement;
    public Text text;
    public LEStatID statType;
    public string statName;
    public int singleValue;
    public int doubleValue;
    public int tripleValue;

	void Awake () {
        ////// DEBUG CODE //////
        if (text == null)
        {
            Debug.Log("ERROR :: Text Object needs to be defined -> LEStatEditor.cs");
        }
        ////////////////////////
        if (singleIncrement != null)
        {
            singleIncrement.parent = this;
            singleIncrement.modifier = singleValue;
        }
        if (doubleIncrement != null)
        {
            doubleIncrement.parent = this;
            doubleIncrement.modifier = doubleValue;
        }
        if (tripleIncrement != null)
        {
            tripleIncrement.parent = this;
            tripleIncrement.modifier = tripleValue;
        }
        if (singleDecrement != null)
        {
            singleDecrement.parent = this;
            singleDecrement.modifier = -singleValue;
        }
        if (doubleDecrement != null)
        {
            doubleDecrement.parent = this;
            doubleDecrement.modifier = -doubleValue;
        }
        if (tripleDecrement != null)
        {
            tripleDecrement.parent = this;
            tripleDecrement.modifier = -tripleValue;
        }
        text.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }
    
    public void TurnOn(LEUnitSettings settings, LEUnitInstance instance)
    {
        currentSettings = settings;
        currentInstance = instance;
        if (singleIncrement != null) singleIncrement.TurnOn();
        if (doubleIncrement != null) doubleIncrement.TurnOn();
        if (tripleIncrement != null) tripleIncrement.TurnOn();
        if (singleDecrement != null) singleDecrement.TurnOn();
        if (doubleDecrement != null) doubleDecrement.TurnOn();
        if (tripleDecrement != null) tripleDecrement.TurnOn();
        text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        text.text = statName + ": " + GetStatValue();
    }

    public void TurnOff()
    {
        if (singleIncrement != null) singleIncrement.TurnOff();
        if (doubleIncrement != null) doubleIncrement.TurnOff();
        if (tripleIncrement != null) tripleIncrement.TurnOff();
        if (singleDecrement != null) singleDecrement.TurnOff();
        if (doubleDecrement != null) doubleDecrement.TurnOff();
        if (tripleDecrement != null) tripleDecrement.TurnOff();
        text.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    public void ChangeState(int val)
    {
        switch(statType)
        {
            case LEStatID.ATTACK:
                {
                    if (currentInstance == null) currentSettings.baseAttack += val;
                    else currentInstance.instanceAttack += val;
                    break;
                }
            case LEStatID.DEFENSE:
                {
                    if (currentInstance == null) currentSettings.baseDefense += val;
                    else currentInstance.instanceDefense += val;
                    break;
                }
            case LEStatID.HEALTH:
                {
                    if (currentInstance == null) currentSettings.baseHealth += val;
                    else currentInstance.instanceHealth += val;
                    break;
                }
            case LEStatID.HIGHRANGE:
                {
                    currentSettings.baseHighRange += val;
                    break;
                }
            case LEStatID.LOWRANGE:
                {
                    currentSettings.baseLowRange += val;
                    break;
                }
            case LEStatID.MANUVERABILITY:
                {
                    currentSettings.baseManuverability += val;
                    break;
                }
            case LEStatID.MOVE:
                {
                    currentSettings.baseMove += val;
                    break;
                }
            case LEStatID.POWER:
                {
                    if (currentInstance == null) currentSettings.basePower += val;
                    else currentInstance.instancePower += val;
                    break;
                }
            case LEStatID.RESISTANCE:
                {
                    if (currentInstance == null) currentSettings.baseResistance += val;
                    else currentInstance.instanceResistance += val;
                    break;
                }
        }
        text.text = statName + ": " + GetStatValue();
    }

    public int GetStatValue()
    {
        if (currentInstance == null)
        {
            return GetSettingsValue();
        }
        return GetInstanceValue();
    }

    private int GetInstanceValue()
    {
        switch (statType)
        {
            case LEStatID.ATTACK: return currentSettings.baseAttack + currentInstance.instanceAttack;
            case LEStatID.DEFENSE: return currentSettings.baseDefense + currentInstance.instanceDefense;
            case LEStatID.HEALTH: return currentSettings.baseHealth + currentInstance.instanceHealth;
            case LEStatID.HIGHRANGE: return currentSettings.baseHighRange;
            case LEStatID.LOWRANGE: return currentSettings.baseLowRange;
            case LEStatID.MANUVERABILITY: return currentSettings.baseManuverability;
            case LEStatID.MOVE: return currentSettings.baseMove;
            case LEStatID.POWER: return currentSettings.basePower + currentInstance.instancePower;
            case LEStatID.RESISTANCE: return currentSettings.baseResistance + currentInstance.instanceResistance;
        }
        return 0;
    }

    private int GetSettingsValue()
    {
        switch (statType)
        {
            case LEStatID.ATTACK: return currentSettings.baseAttack;
            case LEStatID.DEFENSE: return currentSettings.baseDefense;
            case LEStatID.HEALTH: return currentSettings.baseHealth;
            case LEStatID.HIGHRANGE: return currentSettings.baseHighRange;
            case LEStatID.LOWRANGE: return currentSettings.baseLowRange;
            case LEStatID.MANUVERABILITY: return currentSettings.baseManuverability;
            case LEStatID.MOVE: return currentSettings.baseMove;
            case LEStatID.POWER: return currentSettings.basePower;
            case LEStatID.RESISTANCE: return currentSettings.baseResistance;
        }
        return 0;
    }
}

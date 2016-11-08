using UnityEngine;
using UnityEngine.UI;

public class LEStatEditor : MonoBehaviour {
    public LEUnitSettingsEditor reference; // only needed for veterancy button
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
            singleIncrement.statEditorParent = this;
            singleIncrement.modifier = singleValue;
        }
        if (doubleIncrement != null)
        {
            doubleIncrement.statEditorParent = this;
            doubleIncrement.modifier = doubleValue;
        }
        if (tripleIncrement != null)
        {
            tripleIncrement.statEditorParent = this;
            tripleIncrement.modifier = tripleValue;
        }
        if (singleDecrement != null)
        {
            singleDecrement.statEditorParent = this;
            singleDecrement.modifier = -singleValue;
        }
        if (doubleDecrement != null)
        {
            doubleDecrement.statEditorParent = this;
            doubleDecrement.modifier = -doubleValue;
        }
        if (tripleDecrement != null)
        {
            tripleDecrement.statEditorParent = this;
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
            case LEStatID.UNITNAME: break;
            case LEStatID.VETERANCY:
                {
                    if (reference.currentVet + val > -1 && reference.currentVet + val < 4)
                    {
                        reference.currentVet += val;
                        if (currentInstance != null)
                        {
                            currentInstance.instanceVeterancy += val;
                            reference.TurnOn(currentSettings, currentInstance);
                        }
                        else
                        {
                            reference.TurnOn(currentSettings);
                        }
                        break;
                    }
                    break;
                }
            case LEStatID.ATTACK:
                {
                    if (currentInstance == null) currentSettings.baseAttack[reference.currentVet] += val;
                    else currentInstance.instanceAttack += val;
                    break;
                }
            case LEStatID.DEFENSE:
                {
                    if (currentInstance == null) currentSettings.baseDefense[reference.currentVet] += val;
                    else currentInstance.instanceDefense += val;
                    break;
                }
            case LEStatID.HEALTH:
                {
                    if (currentInstance == null) currentSettings.baseHealth[reference.currentVet] += val;
                    else currentInstance.instanceHealth += val;
                    break;
                }
            case LEStatID.HIGHRANGE:
                {
                    currentSettings.baseHighRange[reference.currentVet] += val;
                    break;
                }
            case LEStatID.LOWRANGE:
                {
                    currentSettings.baseLowRange[reference.currentVet] += val;
                    break;
                }
            case LEStatID.MANUVERABILITY:
                {
                    currentSettings.baseManuverability[reference.currentVet] += val;
                    break;
                }
            case LEStatID.MOVE:
                {
                    currentSettings.baseMove[reference.currentVet] += val;
                    break;
                }
            case LEStatID.POWER:
                {
                    if (currentInstance == null) currentSettings.basePower[reference.currentVet] += val;
                    else currentInstance.instancePower += val;
                    break;
                }
            case LEStatID.RESISTANCE:
                {
                    if (currentInstance == null) currentSettings.baseResistance[reference.currentVet] += val;
                    else currentInstance.instanceResistance += val;
                    break;
                }
        }
        text.text = statName + ": " + GetStatValue();
    }

    public string GetStatValue()
    {
        if (currentInstance == null)
        {
            return GetSettingsValue();
        }
        return GetInstanceValue();
    }

    private string GetInstanceValue()
    {
        switch (statType)
        {
            case LEStatID.UNITNAME: return currentSettings.id;
            case LEStatID.VETERANCY: return currentInstance.instanceVeterancy + "";
            case LEStatID.ATTACK: return currentSettings.baseAttack[currentInstance.GetVeterancy()] + currentInstance.instanceAttack + "";
            case LEStatID.DEFENSE: return currentSettings.baseDefense[currentInstance.GetVeterancy()] + currentInstance.instanceDefense + "";
            case LEStatID.HEALTH: return currentSettings.baseHealth[currentInstance.GetVeterancy()] + currentInstance.instanceHealth + "";
            case LEStatID.HIGHRANGE: return currentSettings.baseHighRange[currentInstance.GetVeterancy()] + "";
            case LEStatID.LOWRANGE: return currentSettings.baseLowRange[currentInstance.GetVeterancy()] + "";
            case LEStatID.MANUVERABILITY: return currentSettings.baseManuverability[currentInstance.GetVeterancy()] + "";
            case LEStatID.MOVE: return currentSettings.baseMove[currentInstance.GetVeterancy()] + "";
            case LEStatID.POWER: return currentSettings.basePower[currentInstance.GetVeterancy()] + currentInstance.instancePower + "";
            case LEStatID.RESISTANCE: return currentSettings.baseResistance[currentInstance.GetVeterancy()] + currentInstance.instanceResistance + "";
        }
        return "0";
    }

    private string GetSettingsValue()
    {
        switch (statType)
        {
            case LEStatID.UNITNAME: return currentSettings.id;
            case LEStatID.VETERANCY: return reference.currentVet + "";
            case LEStatID.ATTACK: return currentSettings.baseAttack[reference.currentVet] + "";
            case LEStatID.DEFENSE: return currentSettings.baseDefense[reference.currentVet] + "";
            case LEStatID.HEALTH: return currentSettings.baseHealth[reference.currentVet] + "";
            case LEStatID.HIGHRANGE: return currentSettings.baseHighRange[reference.currentVet] + "";
            case LEStatID.LOWRANGE: return currentSettings.baseLowRange[reference.currentVet] + "";
            case LEStatID.MANUVERABILITY: return currentSettings.baseManuverability[reference.currentVet] + "";
            case LEStatID.MOVE: return currentSettings.baseMove[reference.currentVet] + "";
            case LEStatID.POWER: return currentSettings.basePower[reference.currentVet] + "";
            case LEStatID.RESISTANCE: return currentSettings.baseResistance[reference.currentVet] + "";
        }
        return "0";
    }
}

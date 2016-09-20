using UnityEngine;
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
    public LEStatID statType;
    public string statName;
    public int singleValue;
    public int doubleValue;
    public int tripleValue;

	void Awake () {
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
    }

    public void TurnOff()
    {
        if (singleIncrement != null) singleIncrement.TurnOff();
        if (doubleIncrement != null) doubleIncrement.TurnOff();
        if (tripleIncrement != null) tripleIncrement.TurnOff();
        if (singleDecrement != null) singleDecrement.TurnOff();
        if (doubleDecrement != null) doubleDecrement.TurnOff();
        if (tripleDecrement != null) tripleDecrement.TurnOff();
    }

    public void ChangeState(int val)
    {
        // to be implemented
    }
}

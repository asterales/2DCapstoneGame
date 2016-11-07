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
    public int singleValue;
    public int doubleValue;

    // Use this for initialization
    void Start () {
        currentAI = null;
	}

    void TurnOn()
    {
        // to be implemented
    }

    void TurnOff()
    {
        // to be implemented
    }

    public void ChangeState(int val)
    {
        // to be implemented
    }
}

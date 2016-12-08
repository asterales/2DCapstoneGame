using UnityEngine;
using UnityEngine.UI;

public abstract class VictoryCondition : MonoBehaviour {
    public Text victoryConditionText;
	public abstract bool Achieved(); 
}
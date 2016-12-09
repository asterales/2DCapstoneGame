using UnityEngine;
public class Survive : VictoryCondition {
    public int numTurns;
    void Update()
    {
        victoryConditionText.text = "survive for "+(numTurns-BattleController.instance.numTurns) +" more turns!";
        if (numTurns - BattleController.instance.numTurns==1)
        {
            HexMap.mapArray[25][33].GetComponent<Animator>().runtimeAnimatorController = HexMap.mapArray[25][33].animations[1];
        }
    }
    public override bool Achieved() {
        return BattleController.instance.numTurns == numTurns;
    }
}
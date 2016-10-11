using UnityEngine;
using System.Collections;

public class UnitStats : MonoBehaviour {
    // to be implemented
    public int maxHealth;
    public int health;
    public int attack; //rename to strength? or attack will be calculated based on power also?
    public int defense;
    public int power;
    public int resistance;
    public int mvtRange;

    public static int maxAttack = 100; //rename to strength? or attack will be calculated based on power also?
    public static int maxDefense = 100;
    public static int maxPower = 100;
    public static int maxResistance = 100;
    public MovementDifficulty mvtLevel;
}

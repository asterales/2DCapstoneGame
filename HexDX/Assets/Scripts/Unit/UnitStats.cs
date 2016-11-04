using UnityEngine;
using System.Collections;

public class UnitStats : MonoBehaviour {
    public string className;
    public int maxHealth;
    public int health;
    public int attack; //rename to strength? or attack will be calculated based on power also?
    public int defense;
    public int power;
    public int resistance;
    public int mvtRange;
    public int experience;
    public int veterancy;
    public float zocmodifier;

    public static int maxAttack = 100; //rename to strength? or attack will be calculated based on power also?
    public static int maxDefense = 100;
    public static int maxPower = 100;
    public static int maxResistance = 100;
}

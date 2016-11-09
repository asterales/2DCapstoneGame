using UnityEngine;

public class UnitStats : MonoBehaviour {
    private static readonly float lvlStatModifier = 1.3f;
    private static readonly float lvlHealthModifier = 0.3f;

    public static int maxAttack = 100; 
    public static int maxDefense = 100;
    public static int maxPower = 100;
    public static int maxResistance = 100;

    public string className;
    public int maxHealth;
    public int health;
    public int attack; 
    public int defense;
    public int power;
    public int resistance;
    public int mvtRange;
    public int experience;
    public int veterancy;
    public float zocmodifier;

    public void LevelUp() {
        attack = (int) Mathf.Min(maxAttack, attack * lvlStatModifier);
        defense = (int) Mathf.Min(maxDefense, defense * lvlStatModifier);
        power = (int) Mathf.Min(maxPower, power * lvlStatModifier);
        resistance = (int) Mathf.Min(maxResistance, resistance * lvlStatModifier);
        health += (int)(maxHealth * lvlHealthModifier);
        maxHealth = (int)(maxHealth * lvlStatModifier);
        health = (int) Mathf.Min(maxHealth, health);
    }

    public void LevelDown() {
        attack = (int)(attack / lvlStatModifier);
        defense = (int)(defense / lvlStatModifier);
        power = (int)(power / lvlStatModifier);
        resistance = (int)(resistance / lvlStatModifier);
        maxHealth = (int)(maxHealth / lvlStatModifier);
        health -= (int)(maxHealth * lvlHealthModifier);
    }
}

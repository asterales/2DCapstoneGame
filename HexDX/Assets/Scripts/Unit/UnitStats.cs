using UnityEngine;

public class UnitStats : MonoBehaviour {
    private const float LVL_STAT_MODIFIER = 1.3f;
    private const float LVL_HEALTH_MODIFIER = 0.3f;

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
        attack = (int) Mathf.Min(maxAttack, attack * LVL_STAT_MODIFIER);
        defense = (int) Mathf.Min(maxDefense, defense * LVL_STAT_MODIFIER);
        power = (int) Mathf.Min(maxPower, power * LVL_STAT_MODIFIER);
        resistance = (int) Mathf.Min(maxResistance, resistance * LVL_STAT_MODIFIER);
        health += (int)(maxHealth * LVL_HEALTH_MODIFIER);
        maxHealth = (int)(maxHealth * LVL_STAT_MODIFIER);
        health = (int) Mathf.Min(maxHealth, health);
    }

    public void LevelDown() {
        attack = (int)(attack / LVL_STAT_MODIFIER);
        defense = (int)(defense / LVL_STAT_MODIFIER);
        power = (int)(power / LVL_STAT_MODIFIER);
        resistance = (int)(resistance / LVL_STAT_MODIFIER);
        maxHealth = (int)(maxHealth / LVL_STAT_MODIFIER);
        health -= (int)(maxHealth * LVL_HEALTH_MODIFIER);
    }
}

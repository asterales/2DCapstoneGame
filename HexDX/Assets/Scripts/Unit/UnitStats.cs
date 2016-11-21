using UnityEngine;

public class UnitStats : MonoBehaviour {
    private const float LVL_STAT_MODIFIER = 1.3f;
    private const float LVL_HEALTH_MODIFIER = 0.3f;

    public const int MAX_ATTACK = 100; 
    public const int MAX_DEFENSE = 100;
    public const int MAX_POWER = 100;
    public const int MAX_RESISTANCE = 100;

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
        attack = (int) Mathf.Min(MAX_ATTACK, attack * LVL_STAT_MODIFIER);
        defense = (int) Mathf.Min(MAX_DEFENSE, defense * LVL_STAT_MODIFIER);
        power = (int) Mathf.Min(MAX_POWER, power * LVL_STAT_MODIFIER);
        resistance = (int) Mathf.Min(MAX_RESISTANCE, resistance * LVL_STAT_MODIFIER);
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

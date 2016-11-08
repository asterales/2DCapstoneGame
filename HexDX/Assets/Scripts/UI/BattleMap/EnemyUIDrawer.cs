using UnityEngine;
using UnityEngine.UI;

public class EnemyUIDrawer : MonoBehaviour {

    public static Unit unit;
    public static Image healthbar;
    public static Image portrait;
    public static Image attackbar;
    public static Image defensebar;
    public static Image powerbar;
    public static Image resistbar;
    public static Text hp;
    public static Text attack;
    public static Text defense;
    public static Text power;
    public static Text resist;
    // Use this for initialization
    void Start()
    {
        healthbar = transform.Find("HealthBar").GetComponent<Image>();
        portrait = transform.Find("Portrait").GetComponent<Image>();
        attackbar = transform.Find("AttackBar").GetComponent<Image>();
        defensebar = transform.Find("DefenseBar").GetComponent<Image>();
        powerbar = transform.Find("PowerBar").GetComponent<Image>();
        resistbar = transform.Find("ResistBar").GetComponent<Image>();
        hp = transform.Find("HP").GetComponent<Text>();
        attack = transform.Find("Attack").GetComponent<Text>();
        defense = transform.Find("Defense").GetComponent<Text>();
        power = transform.Find("Power").GetComponent<Text>();
        resist = transform.Find("Resist").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectionController.selectedUnit != null && !SelectionController.selectedUnit.IsPlayerUnit())
        {
            unit = SelectionController.selectedUnit;
        }
        if (SelectionController.target && !SelectionController.target.IsPlayerUnit())
        {
            unit = SelectionController.target;
        }
        DrawUI();

    }

    void DrawUI()
    {
        if (unit)
        {
            portrait.color = Color.white;
            portrait.sprite = unit.sprites.portrait;
            healthbar.fillAmount = (float)unit.Health / (float)unit.MaxHealth;
            attackbar.fillAmount = (float)unit.Attack / (float)UnitStats.maxAttack;
            defensebar.fillAmount = (float)unit.Defense / (float)UnitStats.maxDefense;
            powerbar.fillAmount = (float)unit.Power / (float)UnitStats.maxPower;
            resistbar.fillAmount = (float)unit.Resistance / (float)UnitStats.maxResistance;
            hp.text = unit.Health + "/" + unit.MaxHealth;
            attack.text = "" + unit.Attack;
            defense.text = "" + unit.Defense;
            power.text = "" + unit.Power;
            resist.text = "" + unit.Resistance;
        }
        else
        {
            portrait.color = Color.clear;
            healthbar.fillAmount = 0;
            attackbar.fillAmount = 0;
            defensebar.fillAmount = 0;
            powerbar.fillAmount = 0;
            resistbar.fillAmount = 0;
            hp.text = "";
            attack.text = "";
            defense.text = "";
            power.text = "";
            resist.text = "";
        }
    }
}

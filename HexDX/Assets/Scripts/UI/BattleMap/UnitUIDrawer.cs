using UnityEngine;
using UnityEngine.UI;

public class UnitUIDrawer : MonoBehaviour {
	protected Unit unit;
	private Image portrait;
    private Image healthbar;
    private Image attackbar;
    private Image defensebar;
    private Image powerbar;
    private Image resistbar;
    private Text hp;
    private Text attack;
    private Text defense;
    private Text power;
    private Text resist;
    protected SelectionController sc;

    void Awake() {
        healthbar = transform.Find("HealthBar").GetComponent<Image>();
        portrait = transform.Find("Portrait").GetComponent<Image>();
        attackbar = transform.Find("AttackBar").GetComponent<Image>();
        defensebar = transform.Find("DefenseBar").GetComponent<Image>();
        powerbar = transform.Find("PowerBar").GetComponent<Image>();
        resistbar = transform.Find("ResistBar").GetComponent<Image>();
        portrait = transform.Find("Portrait").GetComponent<Image>();
        hp = transform.Find("HP").GetComponent<Text>();
        attack = transform.Find("Attack").GetComponent<Text>();
        defense = transform.Find("Defense").GetComponent<Text>();
        power = transform.Find("Power").GetComponent<Text>();
        resist = transform.Find("Resist").GetComponent<Text>();
    }

    void Start() {
        sc = SelectionController.instance;
    }

    protected void DrawUI() {
        if (unit) {
            portrait.color = Color.white;
            portrait.sprite = unit.sprites.portrait;
            healthbar.fillAmount = (float)unit.Health / (float)unit.MaxHealth;
            attackbar.fillAmount = (float)unit.Attack / (float)UnitStats.MAX_ATTACK;
            defensebar.fillAmount = (float)unit.Defense / (float)UnitStats.MAX_DEFENSE;
            powerbar.fillAmount = (float)unit.Power / (float)UnitStats.MAX_POWER;
            resistbar.fillAmount = (float)unit.Resistance / (float)UnitStats.MAX_RESISTANCE;
            hp.text = unit.Health + "/" + unit.MaxHealth;
            attack.text = "" + unit.Attack;
            defense.text = "" + unit.Defense;
            power.text = "" + unit.Power;
            resist.text = "" + unit.Resistance;
        } else {
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
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyUIDrawer : MonoBehaviour {

    public Unit unit;
    public Image healthbar;
    public Image portrait;
    public Image attackbar;
    public Image defensebar;
    public Image powerbar;
    public Image resistbar;
    // Use this for initialization
    void Start()
    {
        healthbar = transform.Find("HealthBar").GetComponent<Image>();
        portrait = transform.Find("Portrait").GetComponent<Image>();
        attackbar = transform.Find("AttackBar").GetComponent<Image>();
        defensebar = transform.Find("DefenseBar").GetComponent<Image>();
        powerbar = transform.Find("PowerBar").GetComponent<Image>();
        resistbar = transform.Find("ResistBar").GetComponent<Image>();
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
            attackbar.fillAmount = (float)unit.Attack / 20.0f;
            defensebar.fillAmount = (float)unit.Defense / 20.0f;
            powerbar.fillAmount = (float)unit.Power / 20.0f;
            resistbar.fillAmount = (float)unit.Resistance / 20.0f;
        }
        else
        {
            portrait.color = Color.clear;
            healthbar.fillAmount = 0;
            attackbar.fillAmount = 0;
            defensebar.fillAmount = 0;
            powerbar.fillAmount = 0;
            resistbar.fillAmount = 0;
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class UnitStatDisplay : MonoBehaviour {

    public Unit unit;
    public Image portrait;

    private Image classPanel;
    private Text classText;
    private Image statPanel;
    private Text statText;

    void Awake() {
        Transform classPanelTransform = transform.Find("Class Name");
        classPanel = classPanelTransform.GetComponent<Image>();
        classText = classPanelTransform.Find("Text").GetComponent<Text>();
        Transform statsPanelTransform = transform.Find("Stats");
        statPanel = statsPanelTransform.GetComponent<Image>();
        statText = statsPanelTransform.Find("Text").GetComponent<Text>();
    }

    void Start() {
        ClearDisplay();
    }

    public void DisplayUnit(Unit unitToDisplay) {
        unit = unitToDisplay;
        DisplayStats();
        DisplayPortrait();
    }

    public void ClearDisplay() {
        if(portrait) {
            portrait.color = Color.clear;
        }
        classPanel.enabled = false;
        classText.text = "";
        statPanel.enabled = false;
        statText.text = "";
    }

    private void DisplayStats() {
        if (unit) {
            classPanel.enabled = true;
            classText.text = unit.unitStats.className;
            statPanel.enabled = true;
            statText.text = GetStatsString();
        }
    }

    private void DisplayPortrait() {
        if(portrait) {
            if(unit) {
                portrait.color = Color.white;
                portrait.sprite = unit.sprites.portrait;
            } else {
                portrait.color = Color.clear;
            }
        }
    }

    private string GetStatsString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(string.Format("Health: {0}\n", unit.unitStats.maxHealth));
        sb.Append(string.Format("Attack: {0}\n", unit.unitStats.attack));
        sb.Append(string.Format("Defense: {0}\n", unit.unitStats.defense));
        sb.Append(string.Format("Power: {0}\n", unit.unitStats.power));
        sb.Append(string.Format("Resistance: {0}\n", unit.unitStats.resistance));
        sb.Append(string.Format("Move Range: {0}\n", unit.unitStats.mvtRange));
        sb.Append(string.Format("Move Ability: {0}\n", unit.unitStats.mvtLevel));
        return sb.ToString();
    }
}

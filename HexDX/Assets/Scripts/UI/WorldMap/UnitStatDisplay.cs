using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class UnitStatDisplay : MonoBehaviour {

    public Unit unit;

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

    public void DisplayUnitStats(Unit unitToDisplay) {
        unit = unitToDisplay;
        if (unit) {
            classPanel.enabled = true;
            classText.text = unit.ClassName;
            statPanel.enabled = true;
            statText.text = GetStatsString();
        }
    }

    public void ClearDisplay() {
        classPanel.enabled = false;
        classText.text = "";
        statPanel.enabled = false;
        statText.text = "";
    }

    private string GetStatsString() {
        StringBuilder sb = new StringBuilder();
        sb.Append(string.Format("Health: {0}\n", unit.MaxHealth));
        sb.Append(string.Format("Attack: {0}\n", unit.Attack));
        sb.Append(string.Format("Defense: {0}\n", unit.Defense));
        sb.Append(string.Format("Power: {0}\n", unit.Power));
        sb.Append(string.Format("Resistance: {0}\n", unit.Resistance));
        sb.Append(string.Format("Move Range: {0}\n", unit.MvtRange));
        return sb.ToString();
    }
}

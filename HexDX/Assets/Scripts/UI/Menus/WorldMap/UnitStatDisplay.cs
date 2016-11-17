using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class UnitStatDisplay : MonoBehaviour {

    public Unit unit;

    private Image classPanel;
    private InputField classText;
    private Image statPanel;
    private Text statText;
    private Image unitCard;
    private VeterancyDisplay veterancy;

    void Awake() {
        InitTextPanels();
        InitImages();   
    }

    private void InitTextPanels() {
        Transform classPanelTransform = transform.Find("Class Name");
        classPanel = classPanelTransform.GetComponent<Image>();
        classText = classPanelTransform.Find("Text").GetComponent<InputField>();
        classText.onEndEdit.AddListener(delegate { unit.ClassName = classText.text; });
        Transform statsPanelTransform = transform.Find("Stats");
        statPanel = statsPanelTransform.GetComponent<Image>();
        statText = statsPanelTransform.Find("Text").GetComponent<Text>();
    }

    private void InitImages() {
        Transform unitCardTransform = transform.Find("UnitCard");
        if (unitCardTransform) {
            unitCard = unitCardTransform.GetComponent<Image>();
        }
        Transform vetTransform = transform.Find("Veterancy");
        if (vetTransform) {
            veterancy = vetTransform.GetComponent<VeterancyDisplay>();
        }
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
            if (unitCard) {
                unitCard.sprite = unit.sprites.unitCard;
                unitCard.enabled = true;
            }
            if (veterancy) {
                veterancy.gameObject.SetActive(true);
                veterancy.DisplayVeterancy(unit);
            }
        }
    }

    public void ClearDisplay() {
        classPanel.enabled = false;
        classText.text = "";
        statPanel.enabled = false;
        statText.text = "";
        if (unitCard) {
            unitCard.enabled = false;
        }
        if (veterancy) {
            veterancy.gameObject.SetActive(false);
        }
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

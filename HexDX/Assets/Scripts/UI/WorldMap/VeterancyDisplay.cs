using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VeterancyDisplay : MonoBehaviour {
	private VeterancyStar v1, v2, v3, v4, v5;

	private class VeterancyStar {
		public Image fill;
		public Image empty;

		public VeterancyStar(Transform starTransform) {
			empty = starTransform.GetComponent<Image>();
            fill = starTransform.Find("Fill").GetComponent<Image>();
		}

		public void Fill(float percent) {
			fill.fillAmount = percent;
			empty.color = Color.white;
		}

		public void Hide() {
			fill.fillAmount = 0;
			empty.color = Color.clear;
		}

		public void Full() {
			fill.fillAmount = 1;
			empty.color = Color.clear;
		}
	}

	void Awake() {
		v1 = new VeterancyStar(transform.Find("V1"));
        v2 = new VeterancyStar(v1.empty.transform.Find("V2"));
        v3 = new VeterancyStar(v1.empty.transform.Find("V3"));
		v4 = new VeterancyStar(transform.Find("V4"));
        v5 = new VeterancyStar(v4.empty.transform.Find("V5"));
	}

    public void DisplayVeterancy(Unit unit) {
        switch (unit.Veterancy) {
            case 0:
                v1.Fill((float)unit.Experience / 100.0f);
                v2.Hide();
                v3.Hide();
                v4.Hide();
                v5.Hide();
                break;
            case 1:
                v1.Hide();
                v2.Hide();
                v3.Hide();
                v4.Full();
                v5.Fill((float)unit.Experience / 200.0f);
                break;
            case 2:
                v1.Full();
                v2.Full();
                v3.Fill((float)unit.Experience / 300.0f);
                v4.Hide();
                v5.Hide();
                break;
            default:
                v1.Full();
                v2.Full();
                v3.Full();
                v4.Hide();
                v5.Hide();
                break;
        }
    }
}

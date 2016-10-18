using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RecruitListing : MonoBehaviour {
	private GameObject unitObj;
	public UnitDisplay unitPanel;
	public string unitClass;
	public int cost;

	void Awake() {
		unitPanel = gameObject.GetComponent<UnitDisplay>();
		unitClass = unitClass.Trim();
		unitObj = InstantiateRecruit();
		if (unitObj != null) {
			unitObj.transform.parent = gameObject.transform;
			unitObj.transform.position = GameResources.hidingPosition;
		}
	}

	void Start() {
		if (unitObj != null) {
			unitPanel.unit = unitObj.GetComponent<Unit>();
		}
	}

	public GameObject InstantiateRecruit() {
		return unitClass.Length == 0 ? null :
				Instantiate<GameObject>(Resources.Load<GameObject>("Units/" + unitClass));
	}
}
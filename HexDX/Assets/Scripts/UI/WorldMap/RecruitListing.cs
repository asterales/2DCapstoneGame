using UnityEngine;
using UnityEngine.EventSystems;

public class RecruitListing : MonoBehaviour, IPointerClickHandler {
	private GameObject unitObj;
	public UnitDisplay unitDisplay;
	public string unitClass;
	public int cost;

	public delegate void OnClick(RecruitListing listing);
	public OnClick onClickCallback;

	void Awake() {
		unitDisplay = GetComponent<UnitDisplay>();
		unitClass = unitClass.Trim();
		unitObj = InstantiateRecruit();
		if (unitObj != null) {
			unitObj.transform.parent = gameObject.transform;
			unitObj.transform.position = GameResources.hidingPosition;
		}
	}

	void Start() {
		if (unitObj != null) {
			unitDisplay.unit = unitObj.GetComponent<Unit>();
			GameManager.SetDefaultUnitView(unitDisplay.unit);
		}
	}

	public GameObject InstantiateRecruit() {
		return unitClass.Length == 0 ? null :
				Instantiate<GameObject>(Resources.Load<GameObject>("Units/" + unitClass));
	}

	public void OnPointerClick(PointerEventData eventData) {
		if(eventData.clickCount == 1 && onClickCallback != null) {
			onClickCallback(this);
		}
	} 
}
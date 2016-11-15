using UnityEngine;
using System.Collections.Generic;

public class CustomUnitLoader : MonoBehaviour {
	public bool replacePlayerArmy;
	public List<CustomLoadInfo> units;
	private TutorialInfo info;

	[System.Serializable] // so custom data structure will show up in editor
	public struct CustomLoadInfo {
		public Unit unit;
		public bool isAIUnit;
		public int row;
		public int col;
		public int facing;
	}

	void Awake() {
		info = GetComponent<TutorialInfo>();
	}

	public bool CanLoadUnits() {
		return info != null ? !info.HasBeenCompleted() : true;
	}

	public void LoadUnits() {
		if (HexMap.mapArray != null && HexMap.mapArray.Count > 0) {
			foreach(CustomLoadInfo info in units) {
				AddUnitToMap(info);
			}
		} else {
			Debug.Log("Error: HexMap not initialized to load units - CustomUnitLoader.cs");
		}
	}

	private void AddUnitToMap(CustomLoadInfo loadInfo) {
		Unit unit = loadInfo.unit;
		if (unit) {
			unit.SetTile(HexMap.mapArray[loadInfo.row][loadInfo.col]);
	        unit.facing = loadInfo.facing;
	        unit.SetFacingSprites();
	        if (loadInfo.isAIUnit) {
	            unit.gameObject.AddComponent<BasicUnitAI>();
	        }
		}
    }
}
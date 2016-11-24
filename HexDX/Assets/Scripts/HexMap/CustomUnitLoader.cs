using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CustomUnitLoader : MonoBehaviour {
	public bool replaceArmyIfLoaded;
	public List<CustomLoadInfo> units;
	private TutorialInfo info;
	private bool haveLoadedUnits;

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
		haveLoadedUnits = false;
	}

	public bool CanReplaceUnits() {
		return replaceArmyIfLoaded && haveLoadedUnits;
	}

	public bool CanLoadUnits() {
		return info != null ? !info.HasBeenCompleted() : true;
	}

	public void LoadUnits() {
		if (HexMap.mapArray != null && HexMap.mapArray.Count > 0) {
			Debug.Log("CustomUnitLoader - Loading custom defined units");
			foreach(CustomLoadInfo info in units) {
				AddUnitToMap(info);
			}
			haveLoadedUnits = true;
		} else {
			Debug.Log("Error: HexMap not initialized to load units - CustomUnitLoader.cs");
		}
	}

	public void ReplacePlayerArmy() {
		if (GameManager.instance) {
			List<Unit> playerUnits = units.Where(u => u.unit != null && u.unit.IsPlayerUnit()).Select(u => u.unit).ToList();
			GameManager.instance.ReplaceArmy(playerUnits);
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
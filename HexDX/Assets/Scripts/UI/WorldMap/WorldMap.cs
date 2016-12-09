using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldMap : MonoBehaviour {
	public Territory rootTerritory;
	public Territory finalTerritory;
	public List<Territory> territories;

	void Start() {
		InitTerritoryActivity();
	}

	private void InitTerritoryActivity() {
		rootTerritory.active = true;
		territories = GetComponentsInChildren<Territory>().ToList();
		GameManager gm = GameManager.instance;
		foreach(Territory t in territories) {
			if(gm.defeatedLevelIds.Contains(t.lm.levelId)) {
				t.captured = true;
				t.active = true;
				foreach(Territory n in t.neighbors) {
					n.active = true;
				}
			}
		}
		if (gm && gm.defeatedLevelIds.Count == territories.Count - 1) {
			finalTerritory.gameObject.SetActive(true);
		} else {
			finalTerritory.gameObject.SetActive(false);
		}
	}
}


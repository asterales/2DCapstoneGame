using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour {
	public const int DISPLAY_FACING = 5;
	public const int ACTIVE_UNIT_LIMIT = 8;
	public const int TOTAL_UNIT_LIMIT = 16;
	public static GameManager instance; //singleton

	// public for debugging in editor
	public List<int> defeatedLevelIds;
	public List<Unit> playerAllUnits;
	public List<Unit> activeUnits;
	public int funds; 

	void Awake() {
		if (instance == null) {
			instance = this;
			InitUnitList();
			gameObject.transform.position = GameResources.hidingPosition;
			DontDestroyOnLoad(this.gameObject);
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

	public static void SetDefaultUnitView(Unit unit) {
		unit.facing = DISPLAY_FACING;
		unit.MakeOpen();
	}

	private void InitUnitList() {
		playerAllUnits = new List<Unit>();
		foreach(Unit childUnit in GetComponentsInChildren<Unit>()) {
			AddNewPlayerUnit(childUnit);
		}
		if (playerAllUnits.Count > ACTIVE_UNIT_LIMIT) {
			activeUnits = playerAllUnits.GetRange(0, ACTIVE_UNIT_LIMIT);
		} else {
			activeUnits = playerAllUnits;
		}
		activeUnits.ForEach(u => u.gameObject.SetActive(true));
	}

	public void ClearNullUnits() {
		playerAllUnits = playerAllUnits.Where(u => u != null).ToList();
		activeUnits = activeUnits.Where(u => u != null).ToList();
	}

	public void UpdateArmyAfterBattle() {
		ClearNullUnits();
		foreach(Unit unit in activeUnits) {
			unit.transform.parent = gameObject.transform;
			unit.transform.position = GameResources.hidingPosition;
			unit.Health = unit.MaxHealth;
			SetDefaultUnitView(unit);
		}
	}

	public List<Unit> GetInactiveUnits() {
		return playerAllUnits.Where(u => !activeUnits.Contains(u)).ToList();
	}

	public void AddNewPlayerUnit(Unit unit) {
		playerAllUnits.Add(unit);
		unit.transform.parent = transform;
		unit.transform.position = GameResources.hidingPosition;
		unit.gameObject.SetActive(false);
	}

	void Update() {
		// For debugging
		if (Input.GetKeyDown(KeyCode.Escape)) {
            GameManager.instance.UpdateArmyAfterBattle();
            LevelManager.ReturnToWorldMap();
        }
	}
}
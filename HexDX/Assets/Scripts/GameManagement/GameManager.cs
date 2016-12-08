using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour {
	public const int DISPLAY_FACING = 5;
	public const int ACTIVE_UNIT_LIMIT = 8;
	public const int TOTAL_UNIT_LIMIT = 16;
	public const int MIN_ARMY_SIZE = 1;
	public static GameManager instance; //singleton

	// public for debugging in editor
	public List<int> defeatedLevelIds;
	public List<int> completedTutorials;
	public List<Unit> playerAllUnits;
	public List<Unit> activeUnits;
	public int funds;
    public List<string> randomNames = new List<string> { "Bob", "Steve", "Jerry", "Kyle", "Horatio", "Timmy", "Carl Jr.", "Paul", "Carl", "Toprac", "Dorito", "Mt. Dew" };
    public List<string> deadUnitNames = new List<string>();


    void Awake() {
		if (instance == null) {
			instance = this;
			InitUnitList();
			transform.position = GameResources.hidingPosition;
			DontDestroyOnLoad(gameObject);
		} else if (instance != this) {
			GetComponentsInChildren<Unit>().ToList().ForEach(u => u.gameObject.SetActive(false));
			Destroy(gameObject);
		}
        deadUnitNames = new List<string>();
        deadUnitNames.Add("steve");
	}

	public static void SetDefaultUnitView(Unit unit) {
		unit.facing = DISPLAY_FACING;
		unit.MakeOpen();
	}

	private void InitUnitList() {
		playerAllUnits = new List<Unit>();
		activeUnits = new List<Unit>();
		foreach(Unit childUnit in GetComponentsInChildren<Unit>()) {
			AddNewPlayerUnit(childUnit);
		}
	}

	public bool HasPassedFirstLevel() {
		return defeatedLevelIds.Count > 0;
	}

	public void AddNewPlayerUnit(Unit unit, bool randomName = false) {
		playerAllUnits.Add(unit);
		unit.transform.parent = transform;
        unit.transform.position = GameResources.hidingPosition;
        if (randomName) {
        	unit.ClassName = randomNames[Random.Range(0, randomNames.Count)];
        }
        if (activeUnits.Count < ACTIVE_UNIT_LIMIT) {
        	activeUnits.Add(unit);
        	unit.gameObject.SetActive(true);
        } else {
        	unit.gameObject.SetActive(false);
        }
	}

	public void ResetUnit(Unit unit) {
		unit.transform.parent = transform;
        unit.transform.position = GameResources.hidingPosition;
        unit.Health = unit.MaxHealth;
        unit.DrawHealth();
        SetDefaultUnitView(unit);
	}

	public void UpdateArmyAfterBattle() {
        if (BattleController.instance.PlayerWon) {
            ClearDeadUnits();
            activeUnits.ForEach(u => ResetUnit(u));
        } else {
            RestoreActiveUnits();
        }
	}

    public void ClearDeadUnits()
    {
        foreach (Unit unit in activeUnits)
        {
            if (!unit.enabled)
                deadUnitNames.Add(unit.ClassName);
        }
        playerAllUnits = playerAllUnits.Where(u => u.enabled).ToList();
        activeUnits = activeUnits.Where(u => u.enabled).ToList();
    }


    public void RestoreActiveUnits() {
        foreach (Unit unit in activeUnits) {
            unit.gameObject.SetActive(true);
            unit.enabled = true;
            ResetUnit(unit);
        }
    }

    public void ReplaceArmy(List<Unit> newArmy) {
    	playerAllUnits.ForEach(u => Destroy(u.gameObject));
    	playerAllUnits = newArmy.Where(u => u != null && u.Health > 0).ToList();
    	playerAllUnits.ForEach(u => ResetUnit(u));
    	activeUnits = playerAllUnits.GetRange(0, (int)Mathf.Min(playerAllUnits.Count, ACTIVE_UNIT_LIMIT));
    	activeUnits.ForEach(u => u.gameObject.SetActive(true));
    	GetInactiveUnits().ForEach(u => u.gameObject.SetActive(false));
    }

	public List<Unit> GetInactiveUnits() {
		return playerAllUnits.Where(u => !activeUnits.Contains(u)).ToList();
	}

	public static void DestroyCurrentInstance() {
		GameManager oldInstance = instance;
		instance = null;
		Destroy(oldInstance.gameObject);
	}
}
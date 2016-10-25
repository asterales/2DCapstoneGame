using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// Base class for AI scripts
public abstract class UnitAI : MonoBehaviour {
    public static List<Unit> playerUnits;
	public Unit unit;
	public int unitNum; // index of unit in AIBattleController units list - for debugging purposes
    // to add - reference to deployment area

    public void Start() {
    	unit = GetComponent<Unit>();
    }

    public static void SetPlayerUnits(List<Unit> playerUnitList) {
        playerUnits = playerUnitList;
    }

    public abstract void SetMovement();
    public abstract void SetFacing();
    public abstract void SetAction();
    public abstract void SetAttack();
    
    public virtual void Reset() {
        SelectionController.HideTarget();
    }

    protected Unit GetEnemyInRange() {
        List<Tile> attackTiles = HexMap.GetAttackTiles(unit);
        return playerUnits.FirstOrDefault(playerUnit => playerUnit != null && attackTiles.Contains(playerUnit.currentTile));
    }
}

/*
Current AI setup - each nonplayer unit is attached to an AI script that inherits from this class or a child of this class
    with the above methods implemented.
    AIBattleController will call the above methods to determine the movement, facing, attacking etc. of a particular unit.

Other options
    -interfaces
    -make each phase action is own script and have each unit have a set of AI scripts with one script for each phase
    -have AIBattleController keep a list of scripts and let it pick the one to use for a unit

*/

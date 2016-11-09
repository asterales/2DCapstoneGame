using UnityEngine;
using System.Collections.Generic;

public class UnitState {
    public UnitTurn phase;
    public Tile tile;
    public int facing;
    public int health;
    public int experience;
    public int veterancy;
    public static Dictionary<Unit, UnitState> savedStates = new Dictionary<Unit, UnitState>();
   
   public UnitState(UnitTurn phase, Tile tile, int facing, int health, int experience, int veterancy) {
        this.phase = phase;
        this.tile = tile;
        this.facing = facing;
        this.health = health;
        this.experience = experience;
        this.veterancy = veterancy;
    }

    public static void SaveState(Unit unit) {
        savedStates[unit] = new UnitState(unit.phase, unit.currentTile, unit.facing, unit.Health, unit.Experience, unit.Veterancy);
    }

    public static void RestoreStates() {
        foreach (Unit unit in savedStates.Keys) {
            if (unit) {
                if (unit.Veterancy > savedStates[unit].veterancy) {
                    unit.LevelDown();
                }
                unit.SetTile(savedStates[unit].tile);
                unit.facing = savedStates[unit].facing;
                unit.Health = savedStates[unit].health;
                unit.Experience = savedStates[unit].experience;
                unit.Veterancy = savedStates[unit].veterancy;
                unit.Health = savedStates[unit].health;
                unit.SetPhase(savedStates[unit].phase);
                unit.DrawHealth();
                unit.DrawVeterancy();
            }
        }
    }

    public static void ClearStates() {
        savedStates = new Dictionary<Unit, UnitState>();
    }
}
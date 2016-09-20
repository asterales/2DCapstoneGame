using UnityEngine;
using System.Collections;

public class TileStats : MonoBehaviour {
	public int mvtModifier;
	public int attackModifier;
	public int defenseModifier;
	public int powerModifier;
	public int resistanceModifier;
	public MovementDifficulty mvtDifficulty;
}

public enum MovementDifficulty {
	Easy,
	Medium,
	Hard
};

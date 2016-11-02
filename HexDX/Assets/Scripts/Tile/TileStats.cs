using UnityEngine;
using System.Collections;
using System.Text;

public class TileStats : MonoBehaviour {
	public string typeName;
	public int mvtModifier;
	public int attackModifier;
	public int defenseModifier;
	public int powerModifier;
	public int resistanceModifier;
	public MovementDifficulty mvtDifficulty;

	public string DisplayString { get; private set; }

	void Awake() {
		if (!GetComponent<Tile>().pathable) {
			mvtDifficulty = MovementDifficulty.Unpathable;
		}

		StringBuilder sb = new StringBuilder();
		sb.Append(typeName + "\n");
		sb.Append("Move Difficulty: " + mvtDifficulty + "\n");
		if (mvtModifier != 0) {
			sb.Append("Move Range Mod: " + mvtModifier);
		}
		DisplayString = sb.ToString();
	}
}

public enum MovementDifficulty {
    Free,
	Easy,
	Medium,
	Hard,
	Unpathable
};

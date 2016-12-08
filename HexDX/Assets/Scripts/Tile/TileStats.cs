using UnityEngine;
using System.Text;

public class TileStats : MonoBehaviour {
	public string typeName;
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
		if (attackModifier != 0) {
			sb.Append("Atk: " + GetSignedString(attackModifier) + "\n");
		}
		if (defenseModifier != 0) {
			sb.Append("Def: " + GetSignedString(defenseModifier) + "\n");
		}
		if (powerModifier != 0) {
			sb.Append("Eth: " + GetSignedString(powerModifier) + "\n");
		}
		if (resistanceModifier != 0) {
			sb.Append("Res: " + GetSignedString(resistanceModifier) + "\n");
		}
		sb.Append("Move Difficulty: " + mvtDifficulty);
		DisplayString = sb.ToString();
	}

	private string GetSignedString(int modifier) {
		string sign = modifier > 0 ? "+" : "";
		return sign + modifier;
	}
}

public enum MovementDifficulty {
    Free,
	Easy,
	Medium,
	Hard,
	Unpathable
};

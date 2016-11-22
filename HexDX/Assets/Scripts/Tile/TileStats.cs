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
		sb.Append(typeName);
		if (attackModifier != 0) {
			sb.Append("\nAtk: " + GetSignedString(attackModifier));
		}
		if (defenseModifier != 0) {
			sb.Append("\nDef: " + GetSignedString(defenseModifier));
		}
		if (powerModifier != 0) {
			sb.Append("\nEth: " + GetSignedString(powerModifier));
		}
		if (resistanceModifier != 0) {
			sb.Append("\nRes: " + GetSignedString(resistanceModifier));
		}
		sb.Append("\nMove Difficulty: " + mvtDifficulty);
		sb.Append("\n");
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

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
		StringBuilder sb = new StringBuilder();
		sb.Append(typeName + "\n");
		sb.Append("Mvt Difficulty: " + mvtDifficulty + "\n");
		if (mvtModifier != 0) {
			sb.Append("Mvt Range Modifier: " + mvtModifier);
		}
		DisplayString = sb.ToString();
	}
}

public enum MovementDifficulty {
	Easy,
	Medium,
	Hard
};

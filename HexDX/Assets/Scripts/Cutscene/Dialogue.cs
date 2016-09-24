using UnityEngine;
using System;
using System.Linq;
/* 	Container class for character dialogue lines
	
	Dialogue File txt Format (pipe delimeted) : 
		character_id (int) | expression enum/portrait_index (int) | screen_location enum (int) | spoken_line (string)
		ex 1 | 0 | 1 | Hello
*/

public class Dialogue {
	public string Name { get; private set; }
	public Sprite Portrait { get; private set; }
	public ScreenLocation Side { get; private set; }
	public string Line { get; private set; }

	public Dialogue(string inputLine) {
		string[] tokens = inputLine.Split('|').Select(s => s.Trim()).ToArray();
		if (tokens.Length < 4) {
			throw new ArgumentException("input line must contain 4 elements");
		}
		Character character = CutsceneLoader.characters[int.Parse(tokens[0])];
		Name = character.Name;
		int portraitIndex = int.Parse(tokens[1]);
		Portrait = character.Portraits[portraitIndex];
		Side = (ScreenLocation) int.Parse(tokens[2]);
		Line = tokens[3];
	}

}

public enum ScreenLocation {
	Left,
	Right
}

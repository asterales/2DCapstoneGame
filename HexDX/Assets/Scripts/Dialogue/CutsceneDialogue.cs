using UnityEngine;
using System;
using System.Linq;

/* 	Container class for character cutscene dialogue lines and information
	
	CutsceneDialogue File txt Format (pipe delimeted) : 
		character_id (int) | expression enum/portrait_index (int) | screen_location enum (int) | spoken_line (string)
		cutscene ex: 1 | 0 | 1 | Hello
*/

public class CutsceneDialogue {
	public string CharacterName { get; private set; }
	public Sprite Portrait { get; private set; }
	public ScreenLocation Side { get; private set; }
	public string Line { get; private set; }

	public CutsceneDialogue(string inputLine) {
		string[] tokens = inputLine.Split('|').Select(s => s.Trim()).ToArray();
		if (tokens.Length < 2) {
			throw new ArgumentException("input line must contain 3 or 4 elements");
		}
		Character character = Character.characters[int.Parse(tokens[0])];
		CharacterName = character.Name;
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

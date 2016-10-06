using UnityEngine;
using System;
using System.Linq;

/* 	Container class for character cutscene dialogue lines and information
	
	CutsceneDialogue File txt Format (pipe delimeted) : 
		character_id (int) | expression enum/portrait_index (int) | screen_location enum (int) | spoken_line (string) | alternative displayed name (if not using default) (string)
		cutscene ex: 1 | 0 | 1 | Hello
		cutscene ex with alternative name:  1 | 0 | 1 | Hello | Other Name
*/

public class CutsceneDialogue {
	public string CharacterName { get; private set; }
	public Sprite Portrait { get; private set; }
	public ScreenLocation Side { get; private set; }
	public string Line { get; private set; }

	public CutsceneDialogue(string inputLine) {
		string[] tokens = inputLine.Split('|').Select(s => s.Trim()).ToArray();
		if (tokens.Length < 4) {
			throw new ArgumentException("input line must contain at least 4 elements");
		}
		Character character = Character.characters[int.Parse(tokens[0])];
		if (tokens.Length == 5) {
			CharacterName = tokens[4];
		} else {
			CharacterName = character.Name;
		}
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

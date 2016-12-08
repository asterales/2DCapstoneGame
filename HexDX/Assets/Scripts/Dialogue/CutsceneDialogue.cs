using UnityEngine;
using System;
using System.Linq;

/* 	Container class for character cutscene dialogue lines and information
	
	CutsceneDialogue File txt Format: 
		First line: <background image name>, <bgm>
	
		Lines afterwards: dialogue lines or sfx cues
		Dialogue lines:	
			character_id (int) | expression enum/portrait_index (int) | screen_location enum (int) | spoken_line (string) | alternative displayed name (if not using default) (string) | sfx file to play with line
			cutscene ex: 1 | 0 | 1 | Hello
			cutscene ex with alternative name:  1 | 0 | 1 | Hello | Other Name
			cutscene ex with sfx: 1 | 0 | 1 | Hello || sfx_file_name
			cutscene ex with alt name and sfx: 1 | 0 | 1 | Hello | Other name | sfx_file_name
		Sfx cues:
			sfx_file_name

	Notes: refactor later if later including bgm and background image changes

*/

public class CutsceneDialogue {
	public bool SfxOnly { get; private set; }
	public string SoundFile { get; private set; }

	public string CharacterName { get; private set; }
	public Sprite Portrait { get; private set; }
	public ScreenLocation Side { get; private set; }
	public string Line { get; private set; }

	public CutsceneDialogue(string inputLine) {
		string[] tokens = inputLine.Split('|').Select(s => s.Trim()).ToArray();
		
		// sound effects parsing
		SfxOnly = tokens.Length == 1;
		if (SfxOnly) {
			SoundFile = tokens[0];
		} else {
			Character character = Character.characters[int.Parse(tokens[0])];
			if (tokens[1].Length > 0) {
				int portraitIndex = int.Parse(tokens[1]);
				if (portraitIndex >= 0) {
					Portrait = character.Portraits[portraitIndex];
				}
			}

			Side = (ScreenLocation) int.Parse(tokens[2]);
			Line = tokens[3];

			if (tokens.Length > 4 ) {
				CharacterName = tokens[4];
			} else {
				CharacterName = character.Name;
			}

			if (tokens.Length == 6) {
				SoundFile = tokens[5];
			}
		}
	}
}

public enum ScreenLocation {
	Left,
	Right
}

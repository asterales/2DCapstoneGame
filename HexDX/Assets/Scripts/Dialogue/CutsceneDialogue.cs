using UnityEngine;
using System;
using System.Linq;

/* 	Container class for character cutscene dialogue lines and information
	
	CutsceneDialogue File txt Format:
		All specification for outside files are without extensions 
		First line: <background image name>, <bgm>
			ex. bg image only: background image name>, 
			ex. bgm only: , <bgm>
			ex. both: <background image name>, <bgm>

		Lines afterwards: dialogue lines or sfx cues
		All optional arguments can be left off if at the end of the line, but must still have a delimeter if it is a middle argument
		Dialogue lines:	
			character_id (int, REQUIRED, see characterIds.csv) | expression enum/portrait_index (int, OPTIONAL) | screen_location enum (int, REQUIRED) | spoken_line (string, OPTIONAL) | alternative displayed name (if not using default) (string, OPTIONAL) | sfx file to play with line (string, OPTIONAL) | hide after finish (int 0/1, OPTIONAL, default = no hide = 0)
			cutscene ex: 1 | 0 | 1 | Hello
			cutscene ex with alternative name:  1 | 0 | 1 | Hello | Other Name
			cutscene ex with sfx: 1 | 0 | 1 | Hello || sfx_file_name
			cutscene ex with alt name and sfx: 1 | 0 | 1 | Hello | Other name | sfx_file_name
			cutscene ex with hide dialogue after finish: 1 | 0 | 1 | Hello | Other name | sfx_file_name | 1
		Sfx cues:
			sfx_file_name

		Last line (optiona): side effects
			amount of gold +/-, num units lost (takes lowest health units first)
			ex with only gold: 1000,
			ex with only units: ,1
			ex with both: 1000,1

	Notes: refactor later if later including bgm and background image changes

*/

public class CutsceneDialogue {
	public bool SfxOnly { get; private set; }
	public string SoundFile { get; private set; }
	public bool HideAfterFinish { get; private set; }

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

			// Parse alternate name
			if (tokens.Length > 4 && tokens[4].Length > 0) {
				CharacterName = tokens[4];
			} else {
				CharacterName = character.Name;
			}

			// Parse sound file
			if (tokens.Length > 5 && tokens[5].Length > 0) {
				SoundFile = tokens[5];
			}

			// Parse if hide UI after finish
			if (tokens.Length > 6 && tokens[6].Length > 0) {
				HideAfterFinish = Convert.ToBoolean(Convert.ToInt32(tokens[6]));
			}
		}
	}
}

public enum ScreenLocation {
	Left,
	Right
}

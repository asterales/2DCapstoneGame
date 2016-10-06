using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

/* 	Container class for character data for cutscenes: id, name, portraits
 	
 	Naming conventions (subject to change)
 	Character Bank File CSV format:  
 		character_id (int), character_name (string)
		ex. 1, "Helena Walsh"

	Charater portrait folder: 
		<Character name without whitespace>
		ex. Helena Walsh -> HelenaWalsh
	
	Character portrait file: 
		<character name>_<expression/index number>_<expression name>
		ex. HelenaWalsh_0_Neutral
*/

public class Character {
	private static readonly string portraitsDir = "Portraits/";
	private static readonly string charactersFile = "Cutscenes/CharacterIds";
	public static Dictionary<int, Character> characters = null;

	public int Id { get; private set; }
	public string Name { get; private set; }
	public List<Sprite> Portraits { get; private set; } 

	static Character() {
		LoadCharacterBank();
	}

	private static void LoadCharacterBank() {
		characters = new Dictionary<int, Character>();
		TextAsset idText = Resources.Load<TextAsset>(charactersFile);
		if (idText != null) {
			string[] lines = idText.text.Trim().Split('\n');
			foreach(string line in lines) {
				Character character = new Character(line);
				characters[character.Id] = character;
			} 
		} else {
			Debug.Log("Error: Character file not found: " + charactersFile + " - Character.cs");
		}
	}

	public Character(string csvLine) {
		string[] tokens = csvLine.Split(',');
		if (tokens.Length < 2) {
			throw new ArgumentException(string.Format("csv line has invalid number of items: {0}", csvLine));
		}
		Id = int.Parse(tokens[0]);
		Name = tokens[1].Trim();
		string portraitDir = portraitsDir + new string(Name.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
		Portraits = Resources.LoadAll<Sprite>(portraitDir).ToList();  	
	}
}

/* For readability & standardized indexing of portraits */
public enum Expression {
	Neutral
	//Add other expressions here
}
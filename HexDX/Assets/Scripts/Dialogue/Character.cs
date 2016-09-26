using UnityEngine;
using System;
using System.Linq;
using System.IO;
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
	private static string charactersFile = "Assets/Cutscenes/CharacterIds.csv";
	public static Dictionary<int, Character> characters = null;
	private static string extPattern = "*.jpg"; //can change later

	public int Id { get; private set; }
	public string Name { get; private set; }
	public List<Sprite> Portraits { get; private set; } 

	static Character() {
		LoadCharacterBank(charactersFile);
	}

	private static void LoadCharacterBank(string file) {
		characters = new Dictionary<int, Character>();
		StreamReader reader = new StreamReader(File.OpenRead(file));
		while(!reader.EndOfStream){
			Character character = new Character(reader.ReadLine());
			characters[character.Id] = character;
		}
	}

	public Character(string csvLine){
		string[] tokens = csvLine.Split(',');
		if (tokens.Length < 2) {
			throw new ArgumentException(string.Format("csv line has invalid number of items: {0}", csvLine));
		}
		Id = int.Parse(tokens[0]);
		Name = tokens[1].Trim();
		LoadPortraits();
	}

	private void LoadPortraits() {
		string portraitDir = "Portraits/" + new string(Name.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
		FileInfo[] files = new DirectoryInfo("Assets/Resources/" + portraitDir).GetFiles(extPattern).OrderBy(f => f).ToArray();
		Portraits = new List<Sprite>();
		foreach(FileInfo file in files) {
			string filename = file.Name;
			filename = filename.Remove(filename.IndexOf('.'));
			Portraits.Add(Resources.Load<Sprite>(portraitDir + "/" + filename));
		}	    
	}
}

/* For readability & standardized indexing of portraits */
public enum Expression {
	Neutral
	//Add other expressions here
}
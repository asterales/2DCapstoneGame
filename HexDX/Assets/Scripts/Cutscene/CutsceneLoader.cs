using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CutsceneLoader : MonoBehaviour {
	private static string charactersFile = "Assets/Cutscenes/CharacterIds.csv";
	public static Dictionary<int, Character> characters = null;

	private CutsceneManager cutsceneMgr;
	public string csvCutsceneFile = "Assets/Cutscenes/TestDialogue.txt";

	void Awake() {
		cutsceneMgr = gameObject.GetComponent<CutsceneManager>();
		if (cutsceneMgr == null) {
			Debug.Log("Error :: missing CutsceneManager -> CutsceneLoader.cs");
		}
		if (characters == null) {
			LoadCharacterBank(charactersFile);
		}
		if (csvCutsceneFile != null) {
			LoadCutscene(csvCutsceneFile);
		}
	}

	private static void LoadCharacterBank(string file) {
		characters = new Dictionary<int, Character>();
		StreamReader reader = new StreamReader(File.OpenRead(file));
		while(!reader.EndOfStream){
			Character character = new Character(reader.ReadLine());
			characters[character.Id] = character;
		}
	}

	private void LoadCutscene(string file) {
		Debug.Log(file);
		cutsceneMgr.dialogues = new Queue<Dialogue>();
		StreamReader reader = new StreamReader(File.OpenRead(file));
		while(!reader.EndOfStream){
			cutsceneMgr.dialogues.Enqueue(new Dialogue(reader.ReadLine()));
		}
	}

}


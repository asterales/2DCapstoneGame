Items needed for a cutscenes (subject to change)

Parts of a Dialogue File:
First Line (optional): name of the background image to load without the extension. Background images should be stored in the backgrounds directory
Rest of the file: lines representing the dialogue of the characters (see below for format)

Adding Characters:
1) 	Add desired id and character name (or portrait folder name if its not a character) to the CharacterIds.csv file
	Format: <id>, <character name / folder name>

2) 	Portraits should be contained in a character-specific folder in the Resources/Portraits folder
	Portraits will be loaded based on the character name list in the CharacterIds.csv file. 
	The portraits folder should be the character's name without spaces and the same casing.

Adding Dialogue Files: (*.txt files)
Each line should be pipe delimeted like so:
<character id> | <portrait index> | <screen location/side> | <spoken line> |  <alternative display name>[optional]
	ex. 1 | 0 | 1 | Hello
	ex. 1 | 0 | 1 | Hello | Other Name

Each line of a dialogue file should have the following information:
	1) Character ids (number):
		should match the id found in the CharacterIds.cvs file. 
		This will identify the folder it need to load the portraits from
	2) Portrait index (number):
		the portrait to select from the folder based on its order when the image files are sorted alphabetically. 
		Indexing starts at 0.
	3) Screen locations (number):
		0 for left
		1 for right
	4) Spoken line:
		line(s) to fill the dialogue box
	5) Alternative display name [optional] :
		typically, the default name listed in the CharacterIds.csv file will be used for the name cards. If an alternative name is desired to be displayed, place it as the last element of the line.

How Files are Loaded (for programmers):
Characters and their portraits are loaded on startup into a static dictionary in the Character class.
	Character class constructor parses each line of the CharacterIds.csv file and hold character specific info.
Dialogue files are loaded by CutsceneManager class. 
	CutsceneDialogue class constructor parses each line of a cutscene file and holds info per line of dialogue.

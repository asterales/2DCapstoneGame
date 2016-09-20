using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LEUnitCache : MonoBehaviour {
    public List<LEUnitSettings> unitSettings;
    public List<LEUnitInstance> unitInstances;
    public LEUnitBar unitBar;
    public int numOfTypes;
    
	void Awake () {
        unitSettings = new List<LEUnitSettings>();
        unitInstances = new List<LEUnitInstance>();

        ////// DEBUG CODE //////
        if (unitBar == null)
        {
            Debug.Log("Reference to UnitBar needs to be defined -> LEUnitCache.cs");
        }
        ////////////////////////

        LoadInUnitsFromResources();
	}

    private void LoadInUnitsFromResources()
    {
        // get all the directories for the units
        string path = "Assets\\Resources\\EditorSprites\\Units";
        string[] directories = Directory.GetDirectories(path);

        // each unit will have its own directory
        for (int i = 0; i < directories.Length; i++)
        {
            // get all of the files
            string[] files = Directory.GetFiles(directories[i]);
            // store all the files (ignore the meta files)
            string[] unitFiles = new string[files.Length / 2];
            for (int j = 0; j < files.Length; j += 2)
            {
                // have to cut off "Assets//Resources//" from the path as well as the extension
                unitFiles[j / 2] = files[j].Substring(files[j].Length - (files[j].Length - 17));
                unitFiles[j / 2] = unitFiles[j / 2].Remove(unitFiles[j / 2].IndexOf('.'));
            }

            LEUnitSettings newUnit = this.gameObject.AddComponent<LEUnitSettings>();
            ReadInUnitSettings(newUnit, unitFiles);
        }
    }

    private void ReadInUnitSettings(LEUnitSettings unit, string[] files)
    {
        string settingsFile = "null";
        for (int i=0;i<files.Length;i++)
        {
            if (files[i].Substring(files[i].Length - 4, 4) == ".txt")
            {
                settingsFile = files[i];
            }
        }

        if (settingsFile.Substring(settingsFile.Length - 4, 4) == ".txt")
        {
            TextAsset settings = Resources.Load(settingsFile) as TextAsset;
            unit.InitializeFromText(settings.text);
        }
        else
        {
            unit.CreateFromScratch();
        }
    }

    private void ReadSpritesForUnit(LEUnitSettings unit, string[] files)
    {
        // to be implemented
        // this will cause errors till implemented
        //unit.FindDefaultSprite();
    }
}

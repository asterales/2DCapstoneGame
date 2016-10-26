using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class LEUnitCache : MonoBehaviour
{
    public List<LEUnitSettings> unitSettings;
    public List<LEUnitInstance> unitInstances;
    public LESelectionController selectionController;
    public LEUnitBar unitBar;
    public int numOfTypes;

    void Awake()
    {
        unitSettings = new List<LEUnitSettings>();
        unitInstances = new List<LEUnitInstance>();

        ////// DEBUG CODE //////
        if (unitBar == null)
        {
            Debug.Log("ERROR :: Reference to UnitBar needs to be defined -> LEUnitCache.cs");
        }
        if (selectionController == null)
        {
            Debug.Log("ERROR :: reference to SelectionController needs to be defined -> LEUnitCache.cs");
        }
        ////////////////////////

        LoadInUnitsFromResources();
    }

    private void LoadInUnitsFromResources()
    {
        // get all the directories for the units
        string path = "Assets/Resources/EditorSprites/Units";
        string[] directories = Directory.GetDirectories(path);

        // each unit will have its own directory
        for (int i = 0; i < directories.Length; i++)
        //for (int i = 0; i < 1; i++)
        {
            // get all of the files
            string[] files = Directory.GetFiles(directories[i]);
            // store all the files (ignore the meta files)
            string[] unitFiles = new string[files.Length / 2];
            for (int j = 0; j < files.Length; j += 2)
            {
                // have to cut off "Assets//Resources//" from the path as well as the extension
                unitFiles[j / 2] = files[j].Substring(files[j].Length - (files[j].Length - 17));
                //unitFiles[j / 2] = unitFiles[j / 2].Remove(unitFiles[j / 2].IndexOf('.'));
            }

            LEUnitSettings newUnit = this.gameObject.AddComponent<LEUnitSettings>();
            ReadInUnitSettings(newUnit, unitFiles);
            ReadSpritesForUnit(newUnit, unitFiles);
            unitSettings.Add(newUnit);
        }
    }

    public LEUnitInstance CreateNewUnitInstance(Vector3 position, LEUnitSettings settings)
    {
        GameObject newTile = new GameObject();
        newTile.transform.parent = this.gameObject.transform;
        newTile.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        SpriteRenderer newRenderer = newTile.AddComponent<SpriteRenderer>();
        LEUnitInstance newInstance = newTile.AddComponent<LEUnitInstance>();
        newInstance.selectionController = selectionController;
        newInstance.baseSettings = settings;
        newRenderer.sprite = settings.defaultSprite;
        newTile.transform.position = position;
        newTile.AddComponent<BoxCollider2D>();
        newTile.GetComponent<BoxCollider2D>().size = new Vector2(3f, 2.5f);
        return newInstance;
    }

    private void ReadInUnitSettings(LEUnitSettings unit, string[] files)
    {
        string settingsFile = "null";
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Substring(files[i].Length - 4, 4) == ".txt")
            {
                settingsFile = files[i];
            }
        }

        if (settingsFile.Substring(settingsFile.Length - 4, 4) == ".txt")
        {
            settingsFile = settingsFile.Remove(settingsFile.IndexOf('.'));
            TextAsset settings = Resources.Load(settingsFile) as TextAsset;
            unit.InitializeFromText(settings.text, "Assets/Resources/"+settingsFile);
        }
        else
        {
            unit.CreateFromScratch();
        }
    }

    private void ReadSpritesForUnit(LEUnitSettings unit, string[] files)
    {
        List<string> imageFiles = new List<string>();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Substring(files[i].Length - 4, 4) == ".png")
            {
                imageFiles.Add(files[i]);
            }
        }
        for (int i = 0; i < imageFiles.Count; i++)
        {
            if (imageFiles[i].Substring(imageFiles[i].Length - 4, 4) == ".png")
            {
                string file = imageFiles[i].Remove(imageFiles[i].IndexOf('.'));
                Sprite sprite = Resources.Load<Sprite>(file);
                unit.facingSprites.Add(sprite);
            }
        }

        unit.FindDefaultSprite();
    }

    public LEUnitSettings GetSettingsForId(string id)
    {
        for (int i=0;i<unitSettings.Count;i++)
        {
            if (string.Compare(unitSettings[i].id, id) == 0)
            {
                return unitSettings[i];
            }
        }
        Debug.Log("INPUT ERROR :: Could Not Find UnitSettings");
        return null;
    }
}

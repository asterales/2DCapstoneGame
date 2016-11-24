using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LEMapCache : MonoBehaviour {
    public List<LELevelCache> levels;
    public LEHexMap hexMap;
    public LEUnitCache unitCache;
    public LEDeploymentCache depCache;
    public int currentLevel = 0;

    void Awake () {
        levels = new List<LELevelCache>();
        ////// DEBUG CODE //////
        if (hexMap == null)
        {
            Debug.Log("ERROR :: Reference to HexMap not Defined -> LEMapCache.cs");
        }
        if (unitCache == null)
        {
            Debug.Log("ERROR :: Reference to UnitCache not Defined -> LEMapCache.cs");
        }
        if (depCache == null)
        {
            Debug.Log("ERROR :: Reference to DepCache not Defined -> LEMapCache.cs");
        }
        ////////////////////////
        ReadInAllLevels();
	}

    private void ReadInAllLevels()
    {
        string path = "Assets/Resources/Maps";
        string[] files = Directory.GetFiles(path);

        string[] mapFiles = new string[files.Length / 2];

        for (int j = 0; j < files.Length; j += 2)
        {
            mapFiles[j / 2] = files[j].Substring(files[j].Length - (files[j].Length - 17));
        }

        for (int i = 0; i < mapFiles.Length; i++)
        {
            if (mapFiles[i].Substring(mapFiles[i].Length - 4, 4) == ".csv")
            {
                //Debug.Log("READING MAP FILE :: " + mapFiles[i]);
                mapFiles[i] = mapFiles[i].Remove(mapFiles[i].IndexOf('.'));
                TextAsset mapData = Resources.Load(mapFiles[i]) as TextAsset;
                LELevelCache newLevel = new LELevelCache();
                newLevel.ParseLevel(mapFiles[i], mapData.text);
                levels.Add(newLevel);
            }
        }
        TransitionTo(currentLevel);
    }

    public void WriteAllLevels()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].WriteLevel();
        }
    }

    public void SaveCurrent()
    {
        levels[currentLevel].CacheLevelData(hexMap, unitCache, depCache);
        levels[currentLevel].WriteLevel();
    }

    public void TransitionTo(int index)
    {
        levels[index].InitializeLevel(hexMap, unitCache, depCache);
        currentLevel = index;
    }
}

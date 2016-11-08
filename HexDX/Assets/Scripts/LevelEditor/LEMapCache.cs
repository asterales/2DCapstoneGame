using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LEMapCache : MonoBehaviour {
    public List<LELevelCache> levels;

	void Start () {
        levels = new List<LELevelCache>();
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
                Debug.Log("READING MAP FILE :: " + mapFiles[i]);
                mapFiles[i] = mapFiles[i].Remove(mapFiles[i].IndexOf('.'));
                TextAsset mapData = Resources.Load(mapFiles[i]) as TextAsset;
                LELevelCache newLevel = new LELevelCache();
                newLevel.ParseLevel(mapFiles[i], mapData.text);
                levels.Add(newLevel);
            }
        }
    }

    public void WriteAllLevels()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].WriteLevel();
        }
    }
}

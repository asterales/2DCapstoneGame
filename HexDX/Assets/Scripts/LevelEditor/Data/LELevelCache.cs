using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class LELevelCache {
    public List<LEBaseUnitData> baseUnitData;
    public LEMobCache mobCache;
    public LEBaseDepData baseDepData;
    public LEBaseTileData baseTileData;
    public string id;
    
	public LELevelCache() {
        baseUnitData = new List<LEBaseUnitData>();
        mobCache = new LEMobCache();
        baseTileData = new LEBaseTileData();
        baseDepData = new LEBaseDepData();
        id = "fox.box";
	}

    public void ParseLevel(string fileName, string data)
    {
        id = fileName;
        int row = 0;

        string[] lines = data.Split('\n');
        string[] mapDim = lines[row++].Split(',');

        int hei = Convert.ToInt32(mapDim[0]);
        int wid = Convert.ToInt32(mapDim[1]);

        for (int i = 0; i < hei; i++)
        {
            baseTileData.tileData.Add(new List<int>());
            string[] tileRow = lines[row++].Split(',');
            for (int j = 0; j < wid; j++)
            {
                baseTileData.tileData[i].Add(Convert.ToInt32(tileRow[j]));
            }
        }

        int numUnits = Convert.ToInt32(lines[row++]);

        for (int i = 0; i < numUnits; i++)
        {
            LEBaseUnitData newData = new LEBaseUnitData();
            newData.Parse(lines[row++]);
            baseUnitData.Add(newData);
        }

        int numDeps = Convert.ToInt32(lines[row++]);

        for (int i = 0; i < numDeps; i++)
        {
            LEBaseDepData newData = new LEBaseDepData();
            string[] lineData = lines[row++].Split(',');
            newData.AddDep(Convert.ToInt32(lineData[0]), Convert.ToInt32(lineData[1]));
        }

        Debug.Log("Size :: " + hei + "," + wid + " Units :: " + numUnits + " Deps :: " + numDeps);
    }

    public void WriteLevel()
    {
        string level = "";
        level += baseTileData.ToString();
        level += baseUnitData.Count + "\n";
        for (int i = 0; i < baseUnitData.Count; i++)
        {
            level += baseUnitData[i].ToString();
        }
        level += baseDepData.ToString();
        File.WriteAllText(id, level);
    }

    public void InitializeLevel()
    {
        // to be implemented
    }
}

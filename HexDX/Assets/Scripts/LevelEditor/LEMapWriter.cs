using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LEMapWriter : MonoBehaviour {
    public LEHexMap hexMap;
    public string fileName;

    public void WriteLevel() // very inefficient
    {
        List<List<LETile>> tiles = hexMap.tileArray;
        string data = "";
        for (int i=0;i<tiles.Count;i++)
        {
            for (int j=0;j<tiles[i].Count;j++)
            {
                if (j==tiles[i].Count - 1)
                {
                    data += tiles[i][j].type;
                }
                else
                {
                    data += tiles[i][j].type;
                    data += ",";
                }
            }
            data += "\n";
        }


        File.WriteAllText(fileName, data);
    }
}

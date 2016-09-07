using UnityEngine;
using System.Collections.Generic;

public class LEMapWriter : MonoBehaviour {
    public LEHexMap hexMap;
    public string fileName;

    public void WriteLevel() // very inefficient
    {
        List<List<LETile>> tiles = hexMap.tileArray;
        string data = "";
        for (int i=0;i<tiles.Count;i++)
        {
            for (int j=0;j<tiles[i].Count;i++)
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
    }
}

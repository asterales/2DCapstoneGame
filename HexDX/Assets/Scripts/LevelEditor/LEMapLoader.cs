using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LEMapLoader : MonoBehaviour {
    public LEHexMap hexMap;
    public string fileName;

    public void LoadLevel()
    {
        var reader = new StreamReader(File.OpenRead(fileName));

        for (int i=0;i<12;i++)
        {
            string[] line = reader.ReadLine().Split(',');

            for (int j=0;j<16;j++)
            {
                hexMap.tileArray[i][j].type = int.Parse(line[j]);
                hexMap.tileArray[i][j].ChangeType(int.Parse(line[j]));
            }
        }
    }
}

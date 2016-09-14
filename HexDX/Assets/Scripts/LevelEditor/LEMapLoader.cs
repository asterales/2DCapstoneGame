using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LEMapLoader : MonoBehaviour {
    public LEHexMap hexMap;
    public string fileName;

    public void LoadLevel()
    {
        hexMap.ClearMap();

        var reader = new StreamReader(File.OpenRead(fileName));

        int i = 0;
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        while (!reader.EndOfStream)
        {
            string[] line = reader.ReadLine().Split(',');

            hexMap.AppendRow();

            for (int j=0;j<line.Length;j++)
            {
                Vector3 pos = new Vector3(x, y, z);
                GameObject newTile = hexMap.CreateTileObject(pos, i, j);
                hexMap.AppendTile(newTile);
                x += 2 * hexMap.hexDimension.width;
                hexMap.tileArray[i][j].type = int.Parse(line[j]);
                hexMap.tileArray[i][j].ChangeType(int.Parse(line[j]));
            }

            y -= 2 * hexMap.hexDimension.apex - hexMap.hexDimension.minorApex;
            x -= 2 * hexMap.hexDimension.width * line.Length - hexMap.hexDimension.width;
            z -= .001f;

            Debug.Log(hexMap.hexDimension.width);

            i++;
        }
    }
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LEMapLoader : MonoBehaviour {
    public LEHexMap hexMap;
    public LEUnitCache unitCache;
    public string fileName;

    public void LoadLevel()
    {
        hexMap.ClearMap();

        var reader = new StreamReader(File.OpenRead(fileName));

        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        string[] mapDim = reader.ReadLine().Split(',');

        if (mapDim.Length != 2) Debug.Log("LEVEL FORMAT ERROR :: NO DIMENSIONS FOUND");

        int hei = Convert.ToInt32(mapDim[0]);
        int wid = Convert.ToInt32(mapDim[1]);

        for(int i=0;i<hei;i++)
        {
            string[] line = reader.ReadLine().Split(',');

            hexMap.AppendRow();

            for (int j=0;j<wid;j++)
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
        }

        int numUnits = Convert.ToInt32(reader.ReadLine());

        for(int i=0;i<numUnits;i++)
        {
            // TODO :: Clean up a bit
            string str = reader.ReadLine();
            string[] data = str.Split(',');
            int row = Convert.ToInt32(data[0]);
            int col = Convert.ToInt32(data[1]);
            LEUnitSettings settings = unitCache.GetSettingsForId(data[data.Length-1]);
            LETile tile = hexMap.tileArray[row][col];
            Vector3 newPos = new Vector3(tile.gameObject.transform.position.x, tile.gameObject.transform.position.y, tile.gameObject.transform.position.z - .2f);
            LEUnitInstance instance = unitCache.CreateNewUnitInstance(newPos, settings);
            instance.Read(unitCache, str);
            tile.SetInstance(instance);
            instance.location = tile.position;
            unitCache.unitInstances.Add(instance);
        }

        reader.Close();
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LEMapWriter : MonoBehaviour {
    public LEHexMap hexMap;
    public LEUnitCache unitCache;
    public LEDeploymentCache depCache;
    public string fileName;

    public void WriteUnitSettings()
    {
        // to be implemented
    }

    public void WriteLevel()
    {
        WriteUnitSettings();

        List<List<LETile>> tiles = hexMap.tileArray;
        string data = "";

        data += tiles.Count + "," + tiles[0].Count + "\n";
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

        data += unitCache.unitInstances.Count + "\n";
        for (int i=0;i<unitCache.unitInstances.Count;i++)
        {
            LEUnitInstance unit = unitCache.unitInstances[i];
            data += unit.WriteFull();
        }

        data += depCache.depTiles.Count + "\n";
        for (int i=0;i<depCache.depTiles.Count;i++)
        {
            LEDeploymentTile tile = depCache.depTiles[i];
            data += tile.WriteFull();
        }

        File.WriteAllText(fileName, data);
    }
}

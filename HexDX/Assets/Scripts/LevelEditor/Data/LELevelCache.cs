using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class LELevelCache {
    public List<LEBaseUnitData> baseUnitData;
    public LEMobCache mobCache;
    public LEBaseDepData baseDepData;
    public LEBaseTileData baseTileData;
    public int height;
    public int width;
    public string id;
    public bool dirty;
    
	public LELevelCache() {
        baseUnitData = new List<LEBaseUnitData>();
        mobCache = new LEMobCache();
        baseTileData = new LEBaseTileData();
        baseDepData = new LEBaseDepData();
        id = "fox.box";
        height = 0;
        width = 0;
        dirty = false;
	}

    public void ParseLevel(string fileName, string data)
    {
        id = fileName;
        int row = 0;

        string[] lines = data.Split('\n');
        string[] mapDim = lines[row++].Split(',');

        height = Convert.ToInt32(mapDim[0]);
        width = Convert.ToInt32(mapDim[1]);

        for (int i = 0; i < height; i++)
        {
            baseTileData.tileData.Add(new List<int>());
            string[] tileRow = lines[row++].Split(',');
            for (int j = 0; j < width; j++)
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

        mobCache.ClearCache();

        for (int i = 0; i < baseUnitData.Count; i++)
        {
            if (!mobCache.ContainsID(baseUnitData[i].mobID))
            {
                mobCache.AddMob(baseUnitData[i].mobID, baseUnitData[i].mobType);
            }
            mobCache.GetMobForID(baseUnitData[i].mobID).AddToMob();
        }

        int numDeps = Convert.ToInt32(lines[row++]);

        for (int i = 0; i < numDeps; i++)
        {
            LEBaseDepData newData = new LEBaseDepData();
            string[] lineData = lines[row++].Split(',');
            newData.AddDep(Convert.ToInt32(lineData[0]), Convert.ToInt32(lineData[1]));
        }
    }

    public void WriteLevel()
    {
        string level = "";
        level += baseTileData.ToString();
        level += baseUnitData.Count + "\n";
        Debug.Log("LEVEL :: " + "Resources\\" + id + ".csv");
        for (int i = 0; i < baseUnitData.Count; i++)
        {
            level += baseUnitData[i].ToString();
        }
        level += baseDepData.ToString();
        File.WriteAllText("Assets\\Resources\\"+id+".csv", level);
    }

    public void CacheLevelData(LEHexMap hexMap, LEUnitCache unitCache, LEDeploymentCache depCache)
    {
        baseDepData.ClearData();
        baseUnitData.Clear();
        baseTileData.ClearData();

        List<List<LETile>> tiles = hexMap.tileArray;

        for (int i = 0; i < tiles.Count; i++)
        {
            baseTileData.tileData.Add(new List<int>());
            for (int j = 0; j < tiles[i].Count; j++)
            {
                baseTileData.tileData[i].Add(tiles[i][j].type);
            }
        }

        for (int i = 0; i < unitCache.unitInstances.Count; i++)
        {
            LEUnitInstance unit = unitCache.unitInstances[i];
            baseUnitData.Add(new LEBaseUnitData(unit));
        }

        for (int i = 0; i < depCache.depTiles.Count; i++)
        {
            LEDeploymentTile tile = depCache.depTiles[i];
            baseDepData.AddDep(tile.row, tile.col);
        }
    }

    public void InitializeLevel(LEHexMap hexMap, LEUnitCache unitCache, LEDeploymentCache depCache)
    {
        hexMap.ClearMap();
        unitCache.ClearCache();
        depCache.ClearCache();

        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        for (int i = 0; i < height; i++)
        {
            hexMap.AppendRow();

            for (int j = 0; j < width; j++)
            {
                Vector3 pos = new Vector3(x, y, z);
                GameObject newTile = hexMap.CreateTileObject(pos, i, j);
                hexMap.AppendTile(newTile);
                x += 2 * hexMap.hexDimension.width;
                hexMap.tileArray[i][j].type = baseTileData.tileData[i][j];
                hexMap.tileArray[i][j].ChangeType(baseTileData.tileData[i][j]);
            }

            y -= 2 * hexMap.hexDimension.apex - hexMap.hexDimension.minorApex;
            x -= 2 * hexMap.hexDimension.width * width - hexMap.hexDimension.width;
            z -= .001f;
        }

        for (int i = 0; i < baseUnitData.Count; i++)
        {
            int row = baseUnitData[i].row;
            int col = baseUnitData[i].col;
            LEUnitSettings settings = unitCache.GetSettingsForId(baseUnitData[i].id);

            if (settings != null)
            {
                LETile tile = hexMap.tileArray[row][col];
                Vector3 newPos = new Vector3(tile.gameObject.transform.position.x, tile.gameObject.transform.position.y, tile.gameObject.transform.position.z - .2f);
                LEUnitInstance instance = unitCache.CreateNewUnitInstance(newPos, settings);

                instance.instanceVeterancy = baseUnitData[i].veterancy;
                instance.instanceHealth = baseUnitData[i].health - settings.BaseHealth(instance.instanceVeterancy);
                instance.instanceAttack = baseUnitData[i].attack - settings.BaseAttack(instance.instanceVeterancy);
                instance.instancePower = baseUnitData[i].power - settings.BasePower(instance.instanceVeterancy);
                instance.instanceDefense = baseUnitData[i].defense - settings.BaseDefense(instance.instanceVeterancy);
                instance.instanceResistance = baseUnitData[i].resistance - settings.BaseResistance(instance.instanceVeterancy);
                instance.instanceMobID = baseUnitData[i].mobID;
                instance.instanceMobType = baseUnitData[i].mobType;
                instance.direction = baseUnitData[i].direction;

                tile.SetInstance(instance);
                instance.location = tile.position;
                unitCache.unitInstances.Add(instance);
            }
        }

        for (int i = 0; i < baseDepData.count; i++)
        {
            int row = baseDepData.rowPositions[i];
            int col = baseDepData.colPositions[i];
            hexMap.tileArray[row][col].CreateDeploymentTile();
            hexMap.tileArray[row][col].TurnOffDeployment();
        }
    }
}

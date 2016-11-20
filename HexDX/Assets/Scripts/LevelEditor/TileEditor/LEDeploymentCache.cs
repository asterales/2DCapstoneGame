using UnityEngine;
using System.Collections.Generic;

public class LEDeploymentCache : MonoBehaviour {
    public List<LEDeploymentTile> depTiles;

	void Start () {
        depTiles = new List<LEDeploymentTile>();
	}
	
	public void AddTile(LEDeploymentTile tile, int row, int col)
    {
        tile.row = row;
        tile.col = col;
        depTiles.Add(tile);
    }

    public void RemoveTile(int row, int col)
    {
        for (int i=0;i<depTiles.Count;i++)
        {
            if (depTiles[i].row == row && depTiles[i].col == col)
            {
                depTiles.RemoveAt(i);
            }
        }
    }

    public string WriteData()
    {
        // to be implemented
        return "";
    }

    public void ReadData(LEHexMap map)
    {
        // to be implmeented
    }
}

using System.Collections.Generic;

public class LEBaseTileData {
    public List<List<int>> tileData;

	public LEBaseTileData() {
        tileData = new List<List<int>>();
	}

    public override string ToString()
    {
        string data = "";
        data += tileData.Count + "," + tileData[0].Count + "\n";
        for (int i=0;i<tileData.Count;i++)
        {
            for (int j=0;j<tileData[i].Count;j++)
            {
                if (j==tileData[i].Count-1)
                {
                    data += "" + tileData[i][j];
                }
                else
                {
                    data += tileData[i][j] + ",";
                }
            }
            data += "\n";
        }
        return data;
    }

    public void ClearData()
    {
        tileData.Clear();
    }
}

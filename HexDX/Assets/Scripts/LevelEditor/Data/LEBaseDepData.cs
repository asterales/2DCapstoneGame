using System;
using System.Collections.Generic;

public class LEBaseDepData {
    public List<int> rowPositions;
    public List<int> colPositions;
    public int count;

	public LEBaseDepData()
    {
        rowPositions = new List<int>();
        colPositions = new List<int>();
        count = 0;
    }

    public void AddDep(int row, int col)
    {
        rowPositions.Add(row);
        colPositions.Add(col);
        count++;
    }

    public void ClearData()
    {
        rowPositions.Clear();
        colPositions.Clear();
        count = 0;
    }

    public void Parse(string data)
    {
        string[] lines = data.Split(',');
        int row = Convert.ToInt32(lines[0]);
        int col = Convert.ToInt32(lines[1]);
        AddDep(row, col);
    }

    public override string ToString()
    {
        string data = count + "\n";
        for (int i=0;i<count;i++)
        {
            data += rowPositions[i] + "," + colPositions[i] + "\n";
        }
        return data;
    }
}

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

    public override string ToString()
    {
        return base.ToString();
    }
}

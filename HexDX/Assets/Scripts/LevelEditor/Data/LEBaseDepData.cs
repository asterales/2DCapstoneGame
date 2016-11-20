using System.Collections.Generic;

public class LEBaseDepData {
    public List<int> xPositions;
    public List<int> yPositions;

	public LEBaseDepData()
    {
        xPositions = new List<int>();
        yPositions = new List<int>();
    }

    public void AddDep(int y, int x)
    {
        xPositions.Add(y);
        yPositions.Add(x);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}

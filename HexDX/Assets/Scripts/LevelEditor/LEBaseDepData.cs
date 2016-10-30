using System.Collections.Generic;

public class LEBaseDepData {
    public List<int> xPositions;
    public List<int> yPositions;

	public LEBaseDepData()
    {
        xPositions = new List<int>();
        yPositions = new List<int>();
    }

    public void AddDep(int x, int y)
    {
        xPositions.Add(x);
        yPositions.Add(y);
    }
}

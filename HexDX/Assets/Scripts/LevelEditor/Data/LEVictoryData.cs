public class LEVictoryData {
    public bool routAllEnemies;
    public bool killSpecifiedUnit;
    public bool survival;
    public int turnsToSurvive;
    public int indexOfDesiredUnit;

	public LEVictoryData()
    {
        routAllEnemies = true;
        killSpecifiedUnit = false;
        survival = false;
        turnsToSurvive = 0;
        indexOfDesiredUnit = 0;
	}

    public void Parse(string data)
    {
        // to be implemented
    }
}

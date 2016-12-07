using System;

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

    public void Parse(string[] data)
    {
        int type = Convert.ToInt32(data[0]);
        if (type == 0)
        {
            routAllEnemies = true;
            killSpecifiedUnit = false;
            survival = false;
            turnsToSurvive = 0;
            indexOfDesiredUnit = 0;
        }
        else if (type == 1)
        {
            routAllEnemies = false;
            killSpecifiedUnit = false;
            survival = true;
            turnsToSurvive = Convert.ToInt32(data[1]);
            indexOfDesiredUnit = 0;
        }
        else if (type == 2)
        {
            routAllEnemies = false;
            killSpecifiedUnit = true;
            survival = false;
            turnsToSurvive = 0;
            indexOfDesiredUnit = Convert.ToInt32(data[1]);
        }
    }

    public string Write()
    {
        if (routAllEnemies)
        {
            return "0\n";
        }
        else if (survival)
        {
            return "1," + turnsToSurvive + "\n";
        }
        else if (killSpecifiedUnit)
        {
            return "2," + killSpecifiedUnit + "\n";
        }
        return "0\n";
    }
}

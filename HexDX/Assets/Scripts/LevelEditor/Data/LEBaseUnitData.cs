using System;

public class LEBaseUnitData {
    public int row;
    public int col;
    public int veterancy;
    public int health;
    public int attack;
    public int power;
    public int defense;
    public int resistance;
    public int direction;
    public int mobID;
    public int mobType;
    public string id;

    public LEBaseUnitData()
    {
        row = -1;
        col = -1;
        veterancy = -1;
        health = -1;
        attack = -1;
        power = -1;
        defense = -1;
        resistance = -1;
        direction = -1;
        mobID = -1;
        mobType = -1;
        id = "Cat";
    }

    public LEBaseUnitData(LEUnitInstance instance)
    {
        InitializeFromInstance(instance);
    }

    public void Parse(string data)
    {
        string[] lines = data.Split(',');
        row = Convert.ToInt32(lines[0]);
        col = Convert.ToInt32(lines[1]);
        veterancy = Convert.ToInt32(lines[2]);
        health = Convert.ToInt32(lines[3]);
        attack = Convert.ToInt32(lines[4]);
        power = Convert.ToInt32(lines[5]);
        defense = Convert.ToInt32(lines[6]);
        resistance = Convert.ToInt32(lines[7]);
        mobID = Convert.ToInt32(lines[8]);
        mobType = Convert.ToInt32(lines[9]);
        direction = Convert.ToInt32(lines[10]);
        id = lines[11].Trim();
    }

    private void InitializeFromInstance(LEUnitInstance instance)
    {
        row = instance.location.row;
        col = instance.location.col;
        veterancy = instance.GetVeterancy();
        health = instance.GetHealth();
        attack = instance.GetAttack();
        power = instance.GetPower();
        defense = instance.GetDefense();
        resistance = instance.GetResistence();
        mobID = instance.GetMobID();
        mobType = instance.GetMobType();
        direction = instance.GetDirection();
        id = instance.GetId();
    }

    public override string ToString()
    {
        return base.ToString();
    }
}

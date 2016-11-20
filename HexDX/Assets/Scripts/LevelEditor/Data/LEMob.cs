public class LEMob {
    public int mobID;
    public int mobType;
    public int numInMob;

    public LEMob()
    {
        mobID = 0;
        mobType = 0;
        numInMob = 0;
    }

    public LEMob(int id, int type)
    {
        mobID = id;
        mobType = type;
        numInMob = 0;
    }

    public void AddToMob()
    {
        numInMob++;
    }

    public void RemoveFromMob()
    {
        numInMob--;
    }
}

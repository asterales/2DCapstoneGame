using System.Collections.Generic;

public class LEMobCache {
    public List<LEMob> mobs;

    public LEMobCache()
    {
        mobs = new List<LEMob>();
    }

    public LEMob AddMob(int id, int type)
    {
        mobs.Add(new LEMob(id, type));
        return mobs[mobs.Count - 1];
    }

    public LEMob AddMob()
    {
        mobs.Add(new LEMob(mobs.Count, 0));
        return mobs[mobs.Count - 1];
    }

    public bool ContainsID(int id)
    {
        for (int i = 0; i < mobs.Count; i++)
        {
            if (mobs[i].mobID == id)
            {
                return true;
            }
        }
        return false;
    }

    public LEMob GetMobForID(int id)
    {
        for (int i = 0; i < mobs.Count; i++)
        {
            if (mobs[i].mobID == id)
            {
                return mobs[i];
            }
        }
        return null;
    }

    public int GetMobTypeForID(int id)
    {
        for (int i = 0; i < mobs.Count; i++)
        {
            if (mobs[i].mobID == id)
            {
                return mobs[i].mobType;
            }
        }
        return -1;
    }

    public void CheckUnitsInMob(int id)
    {
        LEMob mob = GetMobForID(id);
        if (mob.numInMob == 0)
        {
            mobs.Remove(mob);
        }
    }

    public void ClearCache()
    {
        mobs.Clear();
    }
}

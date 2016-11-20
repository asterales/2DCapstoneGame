using System.Collections.Generic;

public class LEMobCache {
    public List<LEMob> mobs;

    public LEMobCache()
    {
        mobs = new List<LEMob>();
    }

    public LEMob AddMob()
    {
        mobs.Add(new LEMob(mobs.Count, 0));
        return mobs[mobs.Count - 1];
    }
}

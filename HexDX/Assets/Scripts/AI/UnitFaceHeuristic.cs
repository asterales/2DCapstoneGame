using UnityEngine;
using System.Collections;

public class UnitFaceHeuristic {
    public Unit unit;
    public int weight;

    // weight value interpretation:
    // -- 1 can hit
    // -- 2 can flank
    // -- 3 can sneak

    public UnitFaceHeuristic(Unit u, int w)
    {
        unit = u;
        weight = w;
    }
}

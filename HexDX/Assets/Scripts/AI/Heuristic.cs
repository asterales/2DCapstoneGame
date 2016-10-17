using UnityEngine;
using System.Collections;

public abstract class Heuristic {
    public abstract float CalculateHeuristic(AIWeights weights);
    public abstract void EvaluateData();
}

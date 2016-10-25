public abstract class Heuristic {
    public Tile tile;
    public Unit unit;

    public abstract float CalculateHeuristic(AIWeights weights);
    public abstract void EvaluateData();
}

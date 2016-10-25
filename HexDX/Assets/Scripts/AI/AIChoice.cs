public class AIChoice {
    public TileOption tileChoice;
    public FaceOption faceChoice;
    public AttackOption attackChoice;

    public AIChoice()
    {
        tileChoice = null;
        faceChoice = null;
        attackChoice = null;
    }

    public AIChoice(TileOption tile, FaceOption face, AttackOption attack)
    {
        tileChoice = tile;
        faceChoice = face;
        attackChoice = attack;
    }

    public float Heuristic()
    {
        float heuristic = tileChoice.weight + faceChoice.weight;
        if (attackChoice != null) heuristic += attackChoice.weight;
        return heuristic;
    }
}

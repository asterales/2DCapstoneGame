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
}

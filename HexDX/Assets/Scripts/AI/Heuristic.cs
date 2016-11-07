using UnityEngine;

public abstract class Heuristic {
    public Tile tile;
    public Unit unit;
    public Objective closestObjective;
    public Unit closestEnemyUnit;

    public abstract float CalculateHeuristic(AIWeights weights);
    public abstract void EvaluateData();

    // copied over from Unit
    protected int Facing(Vector2 directionVec)
    {
        int angle = Angle(new Vector2(1, 0), directionVec);
        int facing = -1;
        if (angle < 18 || angle > 342)
        {
            facing = 0;
        }
        else if (angle < 91)
        {
            facing = 1;
        }
        else if (angle < 164)
        {
            facing = 2;
        }
        else if (angle < 198)
        {
            facing = 3;
        }
        else if (angle < 271)
        {
            facing = 4;
        }
        else
        {
            facing = 5;
        }
        return facing;
    }

    protected int Angle(Vector2 from, Vector2 to)
    {
        Vector2 diff = to - from;
        int output = (int)(Mathf.Atan2(diff.y, diff.x) * 57.2957795131f);
        if (output < 0)
            output += 360;
        return output;
    }

    protected float FaceComparison(int faceDirection, int directFaceDirection)
    {
        // maybe make more complex later
        if (faceDirection - directFaceDirection == 0) return 0.0f;
        return 1.0f;
    }
}

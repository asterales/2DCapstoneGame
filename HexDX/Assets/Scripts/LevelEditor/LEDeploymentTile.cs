using UnityEngine;
using System.Collections;

public class LEDeploymentTile : MonoBehaviour {
    public int row;
    public int col;

    public string WriteFull()
    {
        return row + "," + col + "\n";
    }
}

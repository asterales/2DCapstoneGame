using UnityEngine;
using System.Collections;

public class HeuristicIndicator : MonoBehaviour {
    public TextMesh textmesh;
    public MeshRenderer meshrenderer;

    void Awake()
    {
        meshrenderer = this.gameObject.AddComponent<MeshRenderer>();
        meshrenderer.sortingOrder = 3;
        textmesh = this.gameObject.AddComponent<TextMesh>();
        textmesh.fontSize = 15;
        textmesh.text = "potatootatop";
        textmesh.alignment = TextAlignment.Center;
        textmesh.anchor = TextAnchor.LowerCenter;
    }

    public void SetHeuristic(float heuristic)
    {
        textmesh.text = "" + heuristic;
        textmesh.color = Color.blue;
    }

    public void SetHeuristic(string heuristic)
    {
        textmesh.text = heuristic;
        textmesh.color = Color.blue;
    }

    public void TurnOn()
    {
        textmesh.color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
    }

    public void TurnOff()
    {
        textmesh.color = new Color(0.0f, 0.0f, 1.0f, 0.0f);
    }
}

using UnityEngine;
using System.Collections.Generic;

public class LEAICache : MonoBehaviour {
    public List<LEAI> ais;

    void Start()
    {
        ais = new List<LEAI>();
        ReadInAI();
    }

    public void ReadInAI()
    {
        // to be implemented
    }

    public void WriteAI()
    {
        // to be implemented
    }
}

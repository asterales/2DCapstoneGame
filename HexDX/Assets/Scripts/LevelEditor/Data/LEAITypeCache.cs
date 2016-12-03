using UnityEngine;
using System.Collections.Generic;

// Still being implemented

public class LEAITypeCache : MonoBehaviour {
    public List<string> hardCodedAI;
    public List<string> heuristicAI;

    void Start()
    {
        hardCodedAI = new List<string>();
        heuristicAI = new List<string>();
        initHardCodedAI();
        initHeuristicAI();
    }

    private void initHardCodedAI()
    {
        hardCodedAI.Add("Mob");
        hardCodedAI.Add("DefensiveMob");
        hardCodedAI.Add("OffensiveMob");
        hardCodedAI.Add("SuperDefensiveMob");
    }

    private void initHeuristicAI()
    {
        // to be implemented
    }
}

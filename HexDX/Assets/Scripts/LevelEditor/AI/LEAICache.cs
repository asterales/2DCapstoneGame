using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LEAICache : MonoBehaviour {
    public List<LEAI> ais;

    void Start()
    {
        ais = new List<LEAI>();
        ReadInAI();
    }

    public void ReadInAI()
    {
        string path = "Assets/Resources/AIWeights";
        string[] files = Directory.GetFiles(path);

        string[] aiFiles = new string[files.Length / 2];

        for (int j = 0; j < files.Length; j += 2)
        {
            aiFiles[j / 2] = files[j].Substring(files[j].Length - (files[j].Length - 17));
        }

        for (int i = 0; i < aiFiles.Length; i++)
        {
            if (aiFiles[i].Substring(aiFiles[i].Length - 4, 4) == ".txt")
            {
                Debug.Log("READING AI FILE :: " + aiFiles[i]);
                aiFiles[i] = aiFiles[i].Remove(aiFiles[i].IndexOf('.'));
                TextAsset aiData = Resources.Load(aiFiles[i]) as TextAsset;
                LEAI newAI = new LEAI();
                newAI.ParseAI(aiFiles[i], aiData.text);
                ais.Add(newAI);
            }
        }
    }

    public void WriteAI()
    {
        for (int i = 0; i < ais.Count; i++)
        {
            ais[i].WriteAI();
        }
    }
}

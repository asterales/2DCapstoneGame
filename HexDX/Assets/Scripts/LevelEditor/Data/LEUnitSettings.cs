using UnityEngine;
using System;
using System.Collections.Generic;

public class LEUnitSettings : MonoBehaviour {
    private static int numOfVets = 4;
    public List<Sprite> facingSprites;
    public Sprite defaultSprite;
    public int[] baseHealth;
    public int[] baseAttack;
    public int[] basePower;
    public int[] baseDefense;
    public int[] baseResistance;
    public string id;
    public string fileName;

    public int BaseHealth(int vet)
    {
        return baseHealth[vet];
    }

    public int BaseAttack(int vet)
    {
        return baseAttack[vet];
    }

    public int BasePower(int vet)
    {
        return basePower[vet];
    }

    public int BaseDefense(int vet)
    {
        return baseDefense[vet];
    }

    public int BaseResistance(int vet)
    {
        return baseResistance[vet];
    }
    
    void Awake()
    {
        facingSprites = new List<Sprite>();
        defaultSprite = null;
        baseHealth = new int[] { -1, -1, -1, -1 };
        baseAttack = new int[] { -1, -1, -1, -1 };
        basePower = new int[] { -1, -1, -1, -1 };
        baseDefense = new int[] { -1, -1, -1, -1 };
        baseResistance = new int[] { -1, -1, -1, -1 };
        id = "";
    }

    public void CreateFromScratch()
    {
        baseHealth = new int[] { 100, 200, 300, 400 };
        baseAttack = new int[] { 1, 5, 10, 20 };
        basePower = new int[] { 1, 5, 10, 20 };
        baseDefense = new int[] { 1, 5, 10, 20 };
        baseResistance = new int[] { 1, 5, 10, 20 };
        id = ""; // do we needs ids ???
    }

    public void FindDefaultSprite()
    {
        defaultSprite = facingSprites[0];
    }

    // File Format For Settings:
    // for all veterancy levels:
    //   baseHealth
    //   baseAttack
    //   basePower
    //   baseDefense
    //   baseResistance
    // id

    public void InitializeFromText(string data, string file)
    {
        fileName = file + ".txt";
        string[] lines = data.Split('\n');
        if (lines.Length < 6*numOfVets) Debug.Log("NOT ENOUGH LINES -> LEUnitSettings.cs");
        for(int i=0;i<4;i++)
        {
            baseHealth[i] = Convert.ToInt32(lines[0 + 6*i]);
            baseAttack[i] = Convert.ToInt32(lines[1 + 6*i]);
            basePower[i] = Convert.ToInt32(lines[2 + 6*i]);
            baseDefense[i] = Convert.ToInt32(lines[3 + 6*i]);
            baseResistance[i] = Convert.ToInt32(lines[4 + 6*i]);
        }
        id = lines[numOfVets * 6].Trim();
    }

    public string WriteToText()
    {
        string data = "";
        for(int i=0;i<numOfVets; i++)
        {
            data += baseHealth[i] + "\n";
            data += baseAttack[i] + "\n";
            data += basePower[i] + "\n";
            data += baseDefense[i] + "\n";
            data += baseResistance[i] + "\n";
        }
        return data + id + "\n";
    }
}

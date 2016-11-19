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
    public int[] baseMove;
    public int[] mobID;
    public int[] mobType;
    public int[] baseManuverability; // what terrain it can bypass
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

    public int BaseMove(int vet)
    {
        return baseMove[vet];
    }

    public int BaseMobID(int vet)
    {
        return mobID[vet];
    }

    public int BaseMobType(int vet)
    {
        return mobType[vet];
    }

    public int BaseManuverability(int vet)
    {
        return baseManuverability[vet];
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
        baseMove = new int[] { -1, -1, -1, -1 };
        mobID = new int[] { -1, -1, -1, -1 };
        mobType = new int[] { -1, -1, -1, -1 };
        baseManuverability = new int[] { -1, -1, -1, -1 };
        id = "";
    }

    public void CreateFromScratch()
    {
        baseHealth = new int[] { 100, 200, 300, 400 };
        baseAttack = new int[] { 1, 5, 10, 20 };
        basePower = new int[] { 1, 5, 10, 20 };
        baseDefense = new int[] { 1, 5, 10, 20 };
        baseResistance = new int[] { 1, 5, 10, 20 };
        baseMove = new int[] { 5, 5, 5, 5 };
        mobID = new int[] { 1, 1, 1, 1 };
        mobType = new int[] { 1, 1, 1, 1 };
        baseManuverability = new int[] { 1, 1, 1, 1 };
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
    //   baseMove
    //   mobID
    //   mobType
    //   baseManuverability
    // id

    public void InitializeFromText(string data, string file)
    {
        fileName = file + ".txt";
        string[] lines = data.Split('\n');
        if (lines.Length < 9*numOfVets) Debug.Log("NOT ENOUGH LINES -> LEUnitSettings.cs");
        for(int i=0;i<4;i++)
        {
            baseHealth[i] = Convert.ToInt32(lines[0 + 9*i]);
            baseAttack[i] = Convert.ToInt32(lines[1 + 9*i]);
            basePower[i] = Convert.ToInt32(lines[2 + 9*i]);
            baseDefense[i] = Convert.ToInt32(lines[3 + 9*i]);
            baseResistance[i] = Convert.ToInt32(lines[4 + 9*i]);
            baseMove[i] = Convert.ToInt32(lines[5 + 9*i]);
            mobID[i] = Convert.ToInt32(lines[6 + 9*i]);
            mobType[i] = Convert.ToInt32(lines[7 + 9*i]);
            baseManuverability[i] = Convert.ToInt32(lines[8 + 9*i]);
        }
        id = lines[numOfVets * 9].Trim();
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
            data += baseMove[i] + "\n";
            data += mobID[i] + "\n";
            data += mobType[i] + "\n";
            data += baseManuverability[i] + "\n";
        }
        return data + id + "\n";
    }
}

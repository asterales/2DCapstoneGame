using UnityEngine;
using System;
using System.Collections.Generic;

public class LEUnitSettings : MonoBehaviour {
    public List<Sprite> facingSprites;
    public Sprite defaultSprite;
    //public int veterancyLv; // i dont think this is needed
    public int[] baseHealth;
    public int[] baseAttack;
    public int[] basePower;
    public int[] baseDefense;
    public int[] baseResistance;
    public int[] baseMove;
    public int[] baseLowRange;
    public int[] baseHighRange;
    public int[] baseManuverability; // what terrain it can bypass
    public string id;

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

    public int BaseLowRange(int vet)
    {
        return baseLowRange[vet];
    }

    public int BaseHighRange(int vet)
    {
        return baseHighRange[vet];
    }

    public int BaseManuverability(int vet)
    {
        return baseManuverability[vet];
    }
    
    void Awake()
    {
        facingSprites = new List<Sprite>();
        defaultSprite = null;
        //veterancyLv = -1;
        baseHealth = new int[] { -1, -1, -1 };
        baseAttack = new int[] { -1, -1, -1 };
        basePower = new int[] { -1, -1, -1 };
        baseDefense = new int[] { -1, -1, -1 };
        baseResistance = new int[] { -1, -1, -1 };
        baseMove = new int[] { -1, -1, -1 };
        baseLowRange = new int[] { -1, -1, -1 };
        baseHighRange = new int[] { -1, -1, -1 };
        baseManuverability = new int[] { -1, -1, -1 };
        id = "";
    }

    public void CreateFromScratch()
    {
        //veterancyLv = 0;
        baseHealth = new int[] { 100, 200, 300 };
        baseAttack = new int[] { 1, 5, 10 };
        basePower = new int[] { 1, 5, 10 };
        baseDefense = new int[] { 1, 5, 10 };
        baseResistance = new int[] { 1, 5, 10 };
        baseMove = new int[] { 5, 5, 5 };
        baseLowRange = new int[] { 1, 1, 1 };
        baseHighRange = new int[] { 1, 1, 1 };
        baseManuverability = new int[] { 1, 1, 1 };
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
    //   baseLowRange
    //   baseHighRange
    //   baseManuverability
    // id

    public void InitializeFromText(string data)
    {
        string[] lines = data.Split('\n');
        if (lines.Length < 9*3) Debug.Log("NOT ENOUGH LINES -> LEUnitSettings.cs");
        //veterancyLv = Convert.ToInt32(lines[0]);
        for(int i=0;i<3;i++)
        {
            baseHealth[i] = Convert.ToInt32(lines[0 + 9*i]);
            baseAttack[i] = Convert.ToInt32(lines[1 + 9*i]);
            basePower[i] = Convert.ToInt32(lines[2 + 9*i]);
            baseDefense[i] = Convert.ToInt32(lines[3 + 9*i]);
            baseResistance[i] = Convert.ToInt32(lines[4 + 9*i]);
            baseMove[i] = Convert.ToInt32(lines[5 + 9*i]);
            baseLowRange[i] = Convert.ToInt32(lines[6 + 9*i]);
            baseHighRange[i] = Convert.ToInt32(lines[7 + 9*i]);
            baseManuverability[i] = Convert.ToInt32(lines[8 + 9*i]);
        }
        id = lines[3*9];
    }

    public string WriteToText()
    {
        string data = "";
        for(int i=0;i<3;i++)
        {
            data += baseHealth[i] + "\n";
            data += baseAttack[i] + "\n";
            data += basePower[i] + "\n";
            data += baseDefense[i] + "\n";
            data += baseResistance[i] + "\n";
            data += baseMove[i] + "\n";
            data += baseLowRange[i] + "\n";
            data += baseHighRange[i] + "\n";
            data += baseManuverability[i] + "\n";
        }
        return data + id + "\n";
    }
}

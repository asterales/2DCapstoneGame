using UnityEngine;
using System;
using System.Collections.Generic;

public class LEUnitSettings : MonoBehaviour {
    public List<Sprite> facingSprites;
    public Sprite defaultSprite;
    public int baseHealth;
    public int baseAttack;
    public int basePower;
    public int baseDefense;
    public int baseResistance;
    public int baseMove;
    public int baseLowRange;
    public int baseHighRange;
    public int baseManuverability; // what terrain it can bypass
    public int id;

	// Use this for initialization
	void Awake () {
        facingSprites = new List<Sprite>();
        defaultSprite = null;
        baseHealth = -1;
        baseAttack = -1;
        basePower = -1;
        baseDefense = -1;
        baseResistance = -1;
        baseMove = -1;
        baseLowRange = -1;
        baseHighRange = -1;
        baseManuverability = -1;
        id = -1;
	}

    public void CreateFromScratch()
    {
        baseHealth = 100;
        baseAttack = 1;
        basePower = 1;
        baseDefense = 1;
        baseResistance = 1;
        baseMove = 5;
        baseLowRange = 1;
        baseHighRange = 1;
        baseManuverability = 5;
        id = -1; // do we needs ids ???
    }

    public void FindDefaultSprite()
    {
        defaultSprite = facingSprites[0];
    }

    // File Format For Settings:
    // baseHealth
    // baseAttack
    // basePower
    // baseDefense
    // baseResistance
    // baseMove
    // baseLowRange
    // baseHighRange
    // baseManuverability
    // id

    public void InitializeFromText(string data)
    {
        string[] lines = data.Split('\n');
        if (lines.Length < 10) Debug.Log("NOT ENOUGH LINES -> LEUnitSettings.cs");

        baseHealth = Convert.ToInt32(lines[0]);
        baseAttack = Convert.ToInt32(lines[1]);
        basePower = Convert.ToInt32(lines[2]);
        baseDefense = Convert.ToInt32(lines[3]);
        baseResistance = Convert.ToInt32(lines[4]);
        baseMove = Convert.ToInt32(lines[5]);
        baseLowRange = Convert.ToInt32(lines[6]);
        baseHighRange = Convert.ToInt32(lines[7]);
        baseManuverability = Convert.ToInt32(lines[8]);
        id = Convert.ToInt32(lines[9]);
    }

    public string WriteToText()
    {
        return "" +
            baseHealth + "\n" +
            baseAttack + "\n" +
            basePower + "\n" +
            baseDefense + "\n" +
            baseResistance + "\n" +
            baseMove + "\n" +
            baseLowRange + "\n" +
            baseHighRange + "\n" +
            baseManuverability + "\n" +
            id;
    }
}

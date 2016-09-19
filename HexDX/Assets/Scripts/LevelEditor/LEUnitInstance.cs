using UnityEngine;
using System.Collections;

public class LEUnitInstance : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public LEUnitSettings baseSettings;
    public int instanceHealth;
    public int instanceAttack;
    public int instancePower;
    public int instanceDefense;
    public int instanceResistance;
    public int direction;

    void Awake () {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        baseSettings = null;
        instanceHealth = 0;
        instanceAttack = 0;
        instancePower = 0;
        instanceDefense = 0;
        instanceResistance = 0;
        ////// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("Reference to sprite renderer needs to be defined -> LEUnitInstance.cs");
        }
        ////////////////////////
	}

    public void UpdateSprite()
    {
        spriteRenderer.sprite = baseSettings.facingSprites[direction];
    }

    public int GetHealth()
    {
        return baseSettings.baseHealth + instanceHealth;
    }

    public int GetAttack()
    {
        return baseSettings.baseAttack + instanceAttack;
    }

    public int GetDefense()
    {
        return baseSettings.baseDefense + instanceDefense;
    }

    public int GetPower()
    {
        return baseSettings.basePower + instancePower;
    }

    public int GetResistence()
    {
        return baseSettings.baseResistance + instanceResistance;
    }
}

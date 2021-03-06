﻿using UnityEngine;
using System;

public class LEUnitInstance : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public LEUnitSettings baseSettings;
    public LESelectionController selectionController;
    public TileLocation location;
    public int instanceVeterancy;
    public int instanceHealth;
    public int instanceAttack;
    public int instancePower;
    public int instanceDefense;
    public int instanceResistance;
    public int instanceMobID;
    public int instanceMobType;
    public int direction;

    void Awake () {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        baseSettings = null;
        instanceHealth = 0;
        instanceAttack = 0;
        instancePower = 0;
        instanceDefense = 0;
        instanceResistance = 0;
        instanceVeterancy = 0;
        instanceMobID = 0;
        instanceMobType = 0;
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
        return baseSettings.baseHealth[instanceVeterancy] + instanceHealth;
    }

    public int GetAttack()
    {
        return baseSettings.baseAttack[instanceVeterancy] + instanceAttack;
    }

    public int GetDefense()
    {
        return baseSettings.baseDefense[instanceVeterancy] + instanceDefense;
    }

    public int GetPower()
    {
        return baseSettings.basePower[instanceVeterancy] + instancePower;
    }

    public int GetResistence()
    {
        return baseSettings.baseResistance[instanceVeterancy] + instanceResistance;
    }

    public int GetVeterancy()
    {
        return instanceVeterancy; // change once veterancy support is done
    }

    public int GetMobID()
    {
        return instanceMobID;
    }

    public int GetMobType()
    {
        return instanceMobType;
    }

    public string GetId()
    {
        return baseSettings.id;
    }

    public int GetDirection()
    {
        return direction;
    }

    void OnMouseOver()
    {
        // TODO :: MOVE THIS TO LETILE
        if (Input.GetMouseButtonDown(0))
        {
            // select the current
            // update selection for use with units
            //Debug.Log("Clicking");
            selectionController.SetUnitType(this);
        }
        if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("Should Work");
            selectionController.SetUnitType(this);
            selectionController.unitEditor.TurnOff();
            selectionController.unitEditor.TurnOn(baseSettings, this);
        }
    }

    void OnMouseUp()
    {
        // to be implemented
    }

    void OnMouseEnter()
    {
        // to be implemented
    }

    void OnMouseExit()
    {
        // to be implemented
    }

    public string WriteFull()
    {
        return "" +
            location.row + "," +
            location.col + "," +
            GetVeterancy() + "," +
            GetHealth() + "," +
            GetAttack() + "," +
            GetPower() + "," +
            GetDefense() + "," +
            GetResistence() + "," +
            GetMobID() + "," +
            GetMobType() + "," +
            GetDirection() + "," +
            GetId() + "\n";
    }

    public void Read(LEUnitCache unitCache, string str)
    {
        string[] data = str.Split(',');
        int vet = Convert.ToInt32(data[2]);
        int hth = Convert.ToInt32(data[3]);
        int atk = Convert.ToInt32(data[4]);
        int pow = Convert.ToInt32(data[5]);
        int def = Convert.ToInt32(data[6]);
        int res = Convert.ToInt32(data[7]);
        int mid = Convert.ToInt32(data[8]);
        int mty = Convert.ToInt32(data[9]);
        int dir = Convert.ToInt32(data[10]);
        string id = data[11];

        baseSettings = unitCache.GetSettingsForId(id);

        instanceVeterancy = vet;
        instanceHealth = hth - baseSettings.BaseHealth(instanceVeterancy);
        instancePower = pow - baseSettings.BasePower(instanceVeterancy);
        instanceAttack = atk - baseSettings.BaseAttack(instanceVeterancy);
        instanceDefense = def - baseSettings.BaseDefense(instanceVeterancy);
        instanceResistance = res - baseSettings.BaseResistance(instanceVeterancy);
        direction = dir;
    }
}

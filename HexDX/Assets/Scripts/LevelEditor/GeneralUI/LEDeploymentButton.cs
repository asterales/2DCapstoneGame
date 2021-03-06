﻿using UnityEngine;

public class LEDeploymentButton : MonoBehaviour {
    public LESelectionController selectionController;
    public LEHexMap hexMap;
    public LEUnitInstanceButton instanceButton;
    public LEUnitSettingsButton settingsButton;
    public LETileButton tileButton;
    public LEVictoryConditionButton victoryConditionButton;
    private SpriteRenderer spriteRenderer;

	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ////// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: Need reference to sprite renderer -> LEDeploymentButton.cs");
        }
	    if (selectionController == null)
        {
            Debug.Log("ERROR :: Need reference to selection controller -> LEDeploymentButton.cs");
        }
        if (instanceButton == null)
        {
            Debug.Log("ERROR :: Need reference to instance button -> LEDeploymentButton.cs");
        }
        if (settingsButton == null)
        {
            Debug.Log("ERROR :: Need reference to settings button -> LEDeploymentButton.cs");
        }
        if (tileButton == null)
        {
            Debug.Log("ERROR :: Need reference to tile button -> LEDeploymentButton.cs");
        }
        if (victoryConditionButton == null)
        {
            Debug.Log("ERROR :: Need reference to victory condition button -> LEDeploymentButton.cs");
        }
        ////////////////////////
    }

    void OnMouseDown()
    {
        selectionController.SetDepMode();
        if (selectionController.isDepMode)
        {
            Select();
        }
    }

    public void Deselect()
    {
        spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        hexMap.TurnOffDeployment();
    }

    public void Select()
    {
        // deselect other modes
        spriteRenderer.color = Color.white;
        instanceButton.Deselect();
        settingsButton.Deselect();
        tileButton.Deselect();
        victoryConditionButton.Deselect();
        hexMap.TurnOnDeployment();
    }
}

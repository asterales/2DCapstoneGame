﻿using UnityEngine;
using System.Collections.Generic;

public class LEUnitSettingsEditor : MonoBehaviour {
    public LEUnitSettings currentSettings;
    public LEUnitInstance currentInstance;
    public List<LEStatEditor> settingsStatEditors;
    public List<LEStatEditor> instanceStatEditors;
    public LESelectionController selectionController;
    private int onCounter = 10;

    void Awake()
    {
        ////// DEBUG CODE //////
        if (settingsStatEditors.Count == 0)
        {
            Debug.Log("Need references to all StatEditors -> LEUnitSettingsEditor.cs");
        }
        if (instanceStatEditors.Count == 0)
        {
            Debug.Log("Need references to instance StatEditors -> LEUnitSettingsEditor.cs");
        }
        if (selectionController == null)
        {
            Debug.Log("Need reference to selection controller -> LEUnitSettingsEditor.cs");
        }
        ////////////////////////
    }

    void Update()
    {
        if (onCounter < 10) onCounter++;
        else if (Input.GetMouseButtonDown(1))
        {
            TurnOff();
        }
    }

    public void TurnOn(LEUnitSettings settings)
    {
        currentSettings = settings;
        currentInstance = null;
        //Debug.Log("Turning On");
        for (int i = 0; i < settingsStatEditors.Count; i++)
        {
            settingsStatEditors[i].TurnOn(currentSettings, currentInstance);
        }
        onCounter = 0;
    }

    public void TurnOn(LEUnitSettings settings, LEUnitInstance instance)
    {
        currentSettings = settings;
        currentInstance = instance;
        for (int i = 0; i < instanceStatEditors.Count; i++)
        {
            instanceStatEditors[i].TurnOn(currentSettings, currentInstance);
        }
        onCounter = 0;
    }

    public void TurnOff()
    {
        for (int i = 0; i < settingsStatEditors.Count; i++)
        {
            settingsStatEditors[i].TurnOff();
        }
        onCounter = 10;
        selectionController.NullifyMode();
    }
}

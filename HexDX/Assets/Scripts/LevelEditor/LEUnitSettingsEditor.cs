using UnityEngine;
using System.Collections.Generic;

public class LEUnitSettingsEditor : MonoBehaviour {
    public LEUnitSettings currentSettings;
    public LEUnitInstance currentInstance;
    public List<LEStatEditor> statEditors;

    void Awake()
    {
        ////// DEBUG CODE //////
        if (statEditors.Count != 9)
        {
            Debug.Log("Need references to all StatEditors -> LEUnitSettingsEditor.cs");
        }
        ////////////////////////
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TurnOff();
        }
    }

    public void TurnOn(LEUnitSettings settings)
    {
        currentSettings = settings;
        currentInstance = null;
        if (currentSettings != null)
        {
            UpdateTextValues();
        }
    }

    public void TurnOn(LEUnitSettings settings, LEUnitInstance instance)
    {
        currentSettings = settings;
        currentInstance = instance;
        if (currentSettings != null)
        {
            UpdateTextValues();
        }
    }

    public void TurnOff()
    {
        for (int i = 0; i < statEditors.Count; i++)
        {
            statEditors[i].TurnOff();
        }
    }

    public void UpdateTextValues()
    {
        for (int i = 0; i < statEditors.Count; i++)
        {
            statEditors[i].TurnOn(currentSettings, currentInstance);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

public class LEAIStatTable : MonoBehaviour {
    public LEAICache aiCache;
    public LEAI currentAI;
    public List<LEAIStatEditor> aiStatEditors;
    public LESelectionController selectionController;
    private int onCounter = 10;

    void Awake() {
	    ////// DEBUG CODE //////
        if (aiCache == null)
        {
            Debug.Log("ERROR :: Reference to AICache not Defined -> LEAIStatTable.cs");
        }
        if (aiStatEditors.Count == 0)
        {
            Debug.Log("ERROR :: List Of Stats is Empty");
        }
        ////////////////////////
        for(int i = 0; i < aiStatEditors.Count; i++)
        {
            aiStatEditors[i].reference = this;
        }
	}

    public void TurnOn()
    {
        onCounter = 0;
        for (int i = 0; i < aiStatEditors.Count; i++)
        {
            aiStatEditors[i].TurnOn();
        }
    }

    public void TurnOff()
    {
        for (int i = 0; i < aiStatEditors.Count; i++)
        {
            aiStatEditors[i].TurnOff();
        }
        onCounter = 10;
    }

    public void NextAI()
    {
        if (currentAI.index != aiCache.ais.Count - 1)
        {
            currentAI = aiCache.ais[currentAI.index + 1];
        }
    }

    public void PrevAI()
    {
        if (currentAI.index != 0)
        {
            currentAI = aiCache.ais[currentAI.index - 1];
        }
    }

    void Update()
    {
        if (onCounter < 10) onCounter++;
        else if (Input.GetMouseButtonDown(1))
        {
            TurnOff();
        }
    }
}

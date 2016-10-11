using UnityEngine;
using System.Collections;

public class LEUnitButton : MonoBehaviour {
    public LESelectionController selectionController;
    public LEDeploymentButton depButton;
    public LETileButton tileButton;
    public SpriteRenderer spriteRenderer;
    public LEHexMap hexMap;

    void Start()
    {
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
        if (depButton == null)
        {
            Debug.Log("ERROR :: Need reference to unit button -> LEDeploymentButton.cs");
        }
        if (tileButton == null)
        {
            Debug.Log("ERROR :: Need reference to tile button -> LEDeploymentButton.cs");
        }
        ////////////////////////
    }

    void OnMouseDown()
    {
        selectionController.SetUnitMode();
        if (selectionController.isInstanceMode)
        {
            Select();
        }
    }

    public void Deselect()
    {
        spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
    }

    public void Select()
    {
        // deselect other modes
        spriteRenderer.color = Color.white;
        depButton.Deselect();
        tileButton.Deselect();
        hexMap.TurnOnUnit();
    }
}

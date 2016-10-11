using UnityEngine;
using System.Collections;

public class LEDeploymentButton : MonoBehaviour {
    public LESelectionController selectionController;
    public LEHexMap hexMap;
    public LEUnitButton unitButton;
    public LETileButton tileButton;
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
        if (unitButton == null)
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
        selectionController.SetDepMode();
        if (selectionController.isDepMode)
        {
            Select();
            hexMap.TurnOnDeployment();
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
        unitButton.Deselect();
        tileButton.Deselect();
    }
}

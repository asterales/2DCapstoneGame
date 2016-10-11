using UnityEngine;
using System.Collections;

public class LETileButton : MonoBehaviour {
    public LESelectionController selectionController;
    public LEUnitButton unitButton;
    public LEDeploymentButton depButton;
    private SpriteRenderer spriteRenderer;

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
        if (unitButton == null)
        {
            Debug.Log("ERROR :: Need reference to unit button -> LEDeploymentButton.cs");
        }
        if (depButton == null)
        {
            Debug.Log("ERROR :: Need reference to tile button -> LEDeploymentButton.cs");
        }
        ////////////////////////
    }

    void OnMouseDown()
    {
        selectionController.SetSelectTile();
        if (selectionController.isTileMode)
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
        unitButton.Deselect();
        depButton.Deselect();
    }
}

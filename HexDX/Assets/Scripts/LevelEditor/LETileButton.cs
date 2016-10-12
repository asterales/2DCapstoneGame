using UnityEngine;
using System.Collections;

public class LETileButton : MonoBehaviour {
    public LESelectionController selectionController;
    public LEUnitInstanceButton instanceButton;
    public LEUnitSettingsButton settingsButton;
    public LEDeploymentButton depButton;
    public SpriteRenderer spriteRenderer;
    public LEHexMap hexMap;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ////// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: Need reference to sprite renderer -> LETileButton.cs");
        }
        if (selectionController == null)
        {
            Debug.Log("ERROR :: Need reference to selection controller -> LETileButton.cs");
        }
        if (instanceButton == null)
        {
            Debug.Log("ERROR :: Need reference to instance button -> LETileButton.cs");
        }
        if (settingsButton == null)
        {
            Debug.Log("ERROR :: Need reference to settings button -> LETileButton.cs");
        }
        if (depButton == null)
        {
            Debug.Log("ERROR :: Need reference to tile button -> LETileButton.cs");
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
        instanceButton.Deselect();
        settingsButton.Deselect();
        depButton.Deselect();
        hexMap.TurnOnTile();
    }
}

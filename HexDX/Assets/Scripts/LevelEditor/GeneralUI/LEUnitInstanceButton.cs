using UnityEngine;

public class LEUnitInstanceButton : MonoBehaviour {
    public LESelectionController selectionController;
    public LEDeploymentButton depButton;
    public LETileButton tileButton;
    public LEUnitSettingsButton settingsButton;
    public SpriteRenderer spriteRenderer;
    public LEHexMap hexMap;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ////// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: Need reference to sprite renderer -> LEUnitInstanceButton.cs");
        }
        if (selectionController == null)
        {
            Debug.Log("ERROR :: Need reference to selection controller -> LEUnitInstanceButton.cs");
        }
        if (depButton == null)
        {
            Debug.Log("ERROR :: Need reference to unit button -> LEUnitInstanceButton.cs");
        }
        if (tileButton == null)
        {
            Debug.Log("ERROR :: Need reference to tile button -> LEUnitInstanceButton.cs");
        }
        if (settingsButton == null)
        {
            Debug.Log("ERROR :: Need reference to settings button -> LEUnitInstanceButton.cs");
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
        settingsButton.Deselect();
        hexMap.TurnOnUnit();
    }
}

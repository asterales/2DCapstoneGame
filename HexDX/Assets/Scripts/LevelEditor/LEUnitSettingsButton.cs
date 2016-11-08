using UnityEngine;

public class LEUnitSettingsButton : MonoBehaviour {
    public LESelectionController selectionController;
    public LEDeploymentButton depButton;
    public LETileButton tileButton;
    public LEUnitInstanceButton instanceButton;
    public SpriteRenderer spriteRenderer;
    public LEHexMap hexMap;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ////// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: Need reference to sprite renderer -> LEUnitSettingsButton.cs");
        }
        if (selectionController == null)
        {
            Debug.Log("ERROR :: Need reference to selection controller -> LEUnitSettingsButton.cs");
        }
        if (depButton == null)
        {
            Debug.Log("ERROR :: Need reference to unit button -> LEUnitSettingsButton.cs");
        }
        if (tileButton == null)
        {
            Debug.Log("ERROR :: Need reference to tile button -> LEUnitSettingsButton.cs");
        }
        if (instanceButton == null)
        {
            Debug.Log("ERROR :: Need reference to instance button -> LEUnitSettingsButton.cs");
        }
        ////////////////////////
    }

    void OnMouseDown()
    {
        selectionController.SetUnitSettingsMode();
        if (selectionController.isSettingsMode)
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
        instanceButton.Deselect();
        hexMap.TurnOnUnit();
    }
}

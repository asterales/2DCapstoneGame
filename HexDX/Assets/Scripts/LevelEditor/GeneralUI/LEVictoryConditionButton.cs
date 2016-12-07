using UnityEngine;

public class LEVictoryConditionButton : MonoBehaviour
{
    public LESelectionController selectionController;
    public LEDeploymentButton depButton;
    public LETileButton tileButton;
    public LEUnitSettingsButton settingsButton;
    public LEUnitInstanceButton instanceButton;
    public SpriteRenderer spriteRenderer;
    public LEVictoryEditor victoryEditor;
    public LEHexMap hexMap;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ////// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: Need reference to sprite renderer -> LEVictoryConditionButton.cs");
        }
        if (selectionController == null)
        {
            Debug.Log("ERROR :: Need reference to selection controller -> LEVictoryConditionButton.cs");
        }
        if (depButton == null)
        {
            Debug.Log("ERROR :: Need reference to unit button -> LEVictoryConditionButton.cs");
        }
        if (tileButton == null)
        {
            Debug.Log("ERROR :: Need reference to tile button -> LEVictoryConditionButton.cs");
        }
        if (settingsButton == null)
        {
            Debug.Log("ERROR :: Need reference to settings button -> LEVictoryConditionButton.cs");
        }
        if (instanceButton == null)
        {
            Debug.Log("ERROR :: Need reference to instance button -> LEVictoryConditionButton.cs");
        }
        if (hexMap == null)
        {
            Debug.Log("ERROR :: Need reference to HexMap -> LEVictoryConditionButton.cs");
        }
        ////////////////////////
    }

    void OnMouseDown()
    {
        selectionController.SetVCMode();
        if (selectionController.isVCMode)
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
        instanceButton.Deselect();
        victoryEditor.TurnOn();
        hexMap.TurnOnUnit();
    }
}

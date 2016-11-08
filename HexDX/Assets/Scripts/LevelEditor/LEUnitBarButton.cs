using UnityEngine;

public class LEUnitBarButton : MonoBehaviour {
    public SpriteRenderer unitSpriteRenderer;
    public SpriteRenderer backgroundRenderer;
    public LEUnitBar parent;
    public LESelectionController selectionController;
    private LEUnitSettings currentUnit;
    public LEUnitSettingsEditor settingsEditor;
    public int buttonId;

	void Awake () {
        unitSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        currentUnit = null;
        ////// DEBUG CODE //////
        if (unitSpriteRenderer == null)
        {
            Debug.Log("ERROR :: SpriteRenderer needs to be defined -> LETileBarButton.cs");
        }
        if (backgroundRenderer == null)
        {
            Debug.Log("ERROR :: SpriteRenderer for background needs to be defined -> LETileBarButton.cs");
        }
        if (parent == null)
        {
            Debug.Log("ERROR :: Need a reference to the parent -> LETileBarButton.cs");
        }
        if (selectionController == null)
        {
            Debug.Log("ERROR :: Selection Controller needs to be defined -> LETileBarButton.cs");
        }
        if (settingsEditor == null)
        {
            Debug.Log("ERROR :: Horizon Bar needs to be defined -> LETileBarButton.cs");
        }
        ////////////////////////
    }

    void OnMouseOver()
    {
        if (currentUnit == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            // select the current
            // update selection for use with units
            selectionController.SetSettingsType(currentUnit);
        }
        if (Input.GetMouseButtonDown(1))
        {
            settingsEditor.TurnOn(currentUnit);
        }
    }

    public void UpdateButton()
    {
        unitSpriteRenderer.sprite = currentUnit.defaultSprite;
    }

    void OnMouseUp()
    {
        // to be implemented
    }

    void OnMouseEnter()
    {
        // to be implemented
    }

    void OnMouseExit()
    {
        // to be implemented
    }

    public void SetAvatar(LEUnitSettings unit)
    {
        unitSpriteRenderer.sprite = unit.defaultSprite;
        currentUnit = unit;
    }
}

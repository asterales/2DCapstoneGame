using UnityEngine;
using System.Collections;

public class LEDeleteButton : MonoBehaviour {
    public LESelectionController selectionController;
    public SpriteRenderer spriteRenderer;
    public LEUnitCache unitCache;

	void Awake () {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
	    ////// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: Need Reference to Sprite Renderer -> LEDeleteButton.cs");
        }
        if (unitCache == null)
        {
            Debug.Log("ERROR :: Need Reference to Unit Cache -> LEDeleteButton.cs");
        }
        if (selectionController == null)
        {
            Debug.Log("ERROR :: Need Reference to Selection Controller -> LEDeleteButton.cs");
        }
        ////////////////////////
	}

    void OnMouseDown()
    {
        spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f);
        Debug.Log("Trying to Delete");
        if (selectionController.isInstanceMode)
        {
            Debug.Log("Deleting");
            LEUnitInstance selectedUnit = selectionController.selectedUnit;
            selectionController.selectedUnit = null;
            selectionController.NullifyMode();
            unitCache.unitInstances.Remove(selectedUnit);
            Destroy(selectedUnit.gameObject);
        }
    }

    void OnMouseHover()
    {
        spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f);
    }

    void OnMouseEnter()
    {
        spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f);
    }

    void OnMouseUp()
    {
        spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f);
    }

    void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
    }
}

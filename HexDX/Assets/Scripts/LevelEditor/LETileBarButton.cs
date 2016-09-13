using UnityEngine;
using System.Collections;

public class LETileBarButton : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer backgroundRenderer;
    public LESelectionController selectionController;
    public LEHorizonBar horizonBar;
    public int currentTileId;
    public int currentVariantId; // maybe
    public int buttonId;

    void Awake()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        ////// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: SpriteRenderer needs to be defined -> LETileBarButton.cs");
        }
        if (backgroundRenderer == null)
        {
            Debug.Log("ERROR :: SpriteRenderer for background needs to be defined -> LETileBarButton.cs");
        }
        if (selectionController == null)
        {
            Debug.Log("ERROR :: Selection Controller needs to be defined -> LETileBarButton.cs");
        }
        if (horizonBar == null)
        {
            Debug.Log("ERROR :: Horizon Bar needs to be defined -> LETileBarButton.cs");
        }
        ////////////////////////
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // select the current
            //selectionController.
            Debug.Log("Left Mouse Down");
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right Mouse Down");
        }
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

	public void SetAvatar(LESpriteVariantCache cache)
    {
        spriteRenderer.sprite = cache.GetCurrent();
        horizonBar.spriteCache = cache;
    }
}

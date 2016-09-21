using UnityEngine;
using System.Collections;

public class LETileBarButton : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer backgroundRenderer;
    public LETileBar parent;
    public LESelectionController selectionController;
    private LESpriteVariantCache spriteCache;
    public LEHorizonBar horizonBar;
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
        if (parent == null)
        {
            Debug.Log("ERROR :: Need a reference to the parent -> LETileBarButton.cs");
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
        if (spriteCache == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            selectionController.SetSelectTile(spriteCache.id, spriteCache.currentIndex);
        }
        if (Input.GetMouseButtonDown(1))
        {
            parent.TurnOffAllHorizontalBars();
            horizonBar.TurnOn();
        }
    }

    public void UpdateButton()
    {
        spriteRenderer.sprite = spriteCache.GetCurrent();
        selectionController.SetSelectTile(spriteCache.id, spriteCache.currentIndex);
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
        spriteCache = cache;
    }
}

using UnityEngine;
using System.Collections;

public class LETileBarButton : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public int currentTileId;
    public int buttonId;

    void Start()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        ////// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: SpriteRenderer needs to be defined -> LETileBarButton.cs");
        }
        ////////////////////////
    }

	public void SetAvatar(LESpriteVariantCache cache)
    {
        spriteRenderer.sprite = cache.GetCurrent();
    }
}

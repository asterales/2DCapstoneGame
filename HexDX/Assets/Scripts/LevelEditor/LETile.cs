using UnityEngine;
using System.Collections;

public class LETile : MonoBehaviour {
    public LEHexMap reference;
    public LESpriteCache spriteCache;
    public TileLocation position;
    public SpriteRenderer spriteRenderer;
    public int type;
    
    void Awake()
    {
        position = this.gameObject.GetComponent<TileLocation>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        type = 0;

        ////// DEBUG CODE //////
        if (position == null)
        {
            Debug.Log("Error :: The Position Object of the Tile needs to be defined -> LETile.cs");
        }
        if (spriteRenderer == null)
        {
            Debug.Log("Error :: The Sprit Renderer Object of the Tile needs to be defined -> LETile.cs");
        }
        //if (spriteCache == null)
        //{
        //    Debug.Log("Error :: The LE Sprite Cache needs to be set -> LETile.cs");
        //}
        ////////////////////////
    }

    void OnMouseDown()
    {
        // global call to disable expansion
        //LEExpansionController.DisableExpansion();
        type = reference.selectionController.GetTileType();
        spriteRenderer.sprite = spriteCache.GetTileSprite(type);
    }

    void OnMouseEnter()
    {
        // global call to disable expansion
        //LEExpansionController.DisableExpansion();
        if (Input.GetMouseButton(0))
        {
            type = reference.selectionController.GetTileType();
            spriteRenderer.sprite = spriteCache.GetTileSprite(type);
        }
    }

    public void ChangeSprite(Sprite newSprite, int newType)
    {
        spriteRenderer.sprite = newSprite;
        type = newType;
    }

    public void ChangeType(int newType)
    {
        ////// DEBUG CODE //////
        if (spriteCache == null)
        {
            Debug.Log("ERRRRRORORORORO");
        }
        ////////////////////////
        ChangeSprite(spriteCache.GetTileSprite(newType), newType);
    }
}

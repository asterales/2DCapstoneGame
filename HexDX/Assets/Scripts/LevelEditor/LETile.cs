using UnityEngine;
using System.Collections;

public class LETile : MonoBehaviour {
    public LEHexMap reference;
    public LESpriteCache spriteCache;
    public LEUnitCache unitCache;
    public TileLocation position;
    public SpriteRenderer spriteRenderer;
    public int type;
    public static bool canDrag=true;
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
        //if (unitCache == null)
        //{
        //    Debug.Log("Error :: The Unit Cache Object of the Tile needs to be defined -> LETile.cs");
        //}
        //if (spriteCache == null)
        //{
        //    Debug.Log("Error :: The LE Sprite Cache needs to be set -> LETile.cs");
        //}
        ////////////////////////
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            canDrag = false;
        }
    }

    void OnMouseDown()
    {
        // global call to disable expansion
        //LEExpansionController.DisableExpansion();
        if (reference.selectionController.isTileMode)
        {
            type = reference.selectionController.GetTileType();
            spriteRenderer.sprite = spriteCache.GetTileSprite(type);
        }
        if (reference.selectionController.isInstanceMode)
        {
            // does nothing... will move in the future
            // to be implemented
        }
        if (reference.selectionController.isSettingsMode)
        {
            Debug.Log("Placing Unit");
            Vector3 newPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - .2f);
            LEUnitInstance instance = unitCache.CreateNewUnitInstance(newPos, reference.selectionController.selectedSettings);
            unitCache.unitInstances.Add(instance);
        }
        canDrag = true;
    }

    void OnMouseEnter()
    {
        if (reference.selectionController.isTileMode)
        {
            if (Input.GetMouseButton(0) && canDrag)
            {
                type = reference.selectionController.GetTileType();
                spriteRenderer.sprite = spriteCache.GetTileSprite(type);
            }
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

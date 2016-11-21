using UnityEngine;

public class LETile : MonoBehaviour {
    public LEHexMap reference;
    public LESpriteCache spriteCache;
    public LEUnitCache unitCache;
    public LEDeploymentCache depCache;
    public TileLocation position;
    public SpriteRenderer spriteRenderer;
    public int type;
    public static bool canDrag=true;
    private LEUnitInstance currentInstance;
    private GameObject deploymentTile;

    void Awake()
    {
        position = this.gameObject.GetComponent<TileLocation>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        type = 0;
        currentInstance = null;
        deploymentTile = null;

        ////// DEBUG CODE //////
        if (position == null)
        {
            Debug.Log("Error :: The Position Object of the Tile needs to be defined -> LETile.cs");
        }
        if (spriteRenderer == null)
        {
            Debug.Log("Error :: The Sprit Renderer Object of the Tile needs to be defined -> LETile.cs");
        }
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
        Debug.Log("On Mouse Down");
        if (reference.selectionController.isTileMode)
        {
            type = reference.selectionController.GetTileType();
            spriteRenderer.sprite = spriteCache.GetTileSprite(type);
            canDrag = true;
        }
        else if (reference.selectionController.isInstanceMode)
        {
            // does nothing... will move in the future
            // to be implemented
        }
        else if (reference.selectionController.isSettingsMode)
        {
            Vector3 newPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - .001f);
            LEUnitInstance instance = unitCache.CreateNewUnitInstance(newPos, reference.selectionController.selectedSettings);
            currentInstance = instance;
            instance.location = position;
            unitCache.unitInstances.Add(instance);
        }
        else if (reference.selectionController.isDepMode)
        {
            if (deploymentTile == null)
            {
                CreateDeploymentTile();
            }
            else
            {
                DestroyDeploymentTile();
            }
        }
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
        ChangeSprite(spriteCache.GetTileSprite(newType), newType);
    }

    public void PrepareToDestroy()
    {
        DestroyDeploymentTile();
        // delete unit instance
    }

    public void SetInstance(LEUnitInstance instance)
    {
        currentInstance = instance;
    }

    public void CreateDeploymentTile()
    {
        GameObject newTile = new GameObject("Deployment Tile");
        newTile.transform.parent = this.gameObject.transform;
        Vector3 pos = this.gameObject.transform.position;
        newTile.transform.position = new Vector3(pos.x, pos.y, pos.z - 0.001f);
        depCache.AddTile(newTile.AddComponent<LEDeploymentTile>(), position.row, position.col);
        SpriteRenderer tileSpriteRenderer = newTile.AddComponent<SpriteRenderer>();
        tileSpriteRenderer.sprite = reference.deploymentSprite;
        deploymentTile = newTile;
    }

    public void DestroyDeploymentTile()
    {
        depCache.RemoveTile(position.row, position.col);
        Destroy(deploymentTile);
        deploymentTile = null;
    }

    public void TurnOnDeployment()
    {
        if (deploymentTile != null)
        {
            Vector3 position = this.gameObject.transform.position;
            deploymentTile.transform.position = new Vector3(position.x, position.y, position.z - 0.001f);
        }
    }

    public void TurnOffDeployment()
    {
        if (deploymentTile != null)
        {
            Vector3 position = this.gameObject.transform.position;
            deploymentTile.transform.position = new Vector3(position.x, position.y, position.z + 0.001f);
        }
    }

    public void TurnOnTile()
    {
        TurnOffDeployment();
        TurnOffUnit();
    }

    public void TurnOnUnit()
    {
        if (currentInstance != null)
        {
            Vector3 position = this.gameObject.transform.position;
            currentInstance.transform.position = new Vector3(position.x, position.y, position.z - 0.001f);
        }
    }

    public void TurnOffUnit()
    {
        if (currentInstance != null)
        {
            Vector3 position = this.gameObject.transform.position;
            currentInstance.transform.position = new Vector3(position.x, position.y, position.z + 0.001f);
        }
    }
}

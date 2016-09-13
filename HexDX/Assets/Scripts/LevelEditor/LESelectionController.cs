using UnityEngine;
using System.Collections;

public class LESelectionController : MonoBehaviour {
    // cache for tiles
    public LESpriteCache tileSpriteCache;
    public int selectedTileVariantID;
    public int selectedTileID;
    // cache for units
    public LESpriteCache unitSpriteCache;
    public int selectedUnitVariantID;
    public int selectedUnitID;
    // cache for maps
    public LESpriteCache mapSpriteCache;
    public int selectedMapID;

    void Start()
    {
        selectedTileVariantID = 0;
        selectedTileID = 0;

        selectedUnitVariantID = 0;
        selectedUnitID = 0;

        selectedMapID = 0;

        // for now this works, but eventually there will be 3 separate sprite caches
        tileSpriteCache = this.gameObject.GetComponent<LESpriteCache>();
        ////// DEBUG CODE //////
        if (tileSpriteCache == null)
        {
            Debug.Log("ERROR :: Tile Sprite Cache Needs to be defined -> LESelectionController.cs");
        }
        if (unitSpriteCache == null)
        {
            // For future sprint
            //Debug.Log("ERROR :: Unit Sprite Cache Needs to be defined -> LESelectionController.cs");
        }
        if (mapSpriteCache == null)
        {
            // For future sprint
            //Debug.Log("ERROR :: Map Sprite Cache Needs to be defined -> LESelectionController.cs");
        }
        ////////////////////////
    }

    public int GetTileType()
    {
        // we support up to 100 different variants of one tile type
        // first 2 digits (base 10) represent the variant id
        // the rest represent the type of tile
        return selectedTileID * 100 + selectedTileVariantID;
    }

    public int GetUnitType()
    {
        // to be implemented next sprint
        return 0;
    }

    public int GetMapType()
    {
        // to be implemented next sprint
        return 0;
    }

    // OLD STUFF, KEEPING FOR REFERENCE

    /*public GameObject buttonSelectObj;
    public LETileButton selectedTileButton;
    
	void Start () {
        selectedTileButton = null;
        ////// DEBUG CODE //////
        if (buttonSelectObj == null)
        {
            Debug.Log("Button Select Obj needs to be defined -> LESelectionController.cs");
        }
        ////////////////////////
	}
	
	void Update () {
	    if (selectedTileButton == null)
        {
            //buttonSelectObj.transform.position = new Vector3(-1000, -1000, 0.9f);
        }
        else
        {
            Vector3 temp = selectedTileButton.transform.position;
            //buttonSelectObj.transform.position = new Vector3(temp.x, temp.y, 0.9f);
        }
	}*/
}

using UnityEngine;
using System.Collections;

public class LESelectionController : MonoBehaviour {
    // cache for tiles
    public LESpriteCache tileSpriteCache;
    public int selectedTileVariantID;
    public int selectedTileID;
    // cache for units
    public LEUnitCache unitCache;
    public int selectedUnitCacheID;
    // currently selected unitSettings
    public LEUnitSettings selectedSettings;
    // currently selected unit
    public LEUnitInstance selectedUnit;
    //public int selectedUnitID;
    public LEUnitSettingsEditor unitEditor;
    // cache for maps
    public LESpriteCache mapSpriteCache;
    public int selectedMapID;

    public bool isTileMode;
    public bool isSettingsMode;
    public bool isInstanceMode;

    void Start()
    {
        selectedTileVariantID = 0;
        selectedTileID = 0;

        selectedUnitCacheID = 0;

        selectedMapID = 0;

        // for now this works, but eventually there will be 3 separate sprite caches
        tileSpriteCache = this.gameObject.GetComponent<LESpriteCache>();
        ////// DEBUG CODE //////
        if (tileSpriteCache == null)
        {
            Debug.Log("ERROR :: Tile Sprite Cache Needs to be defined -> LESelectionController.cs");
        }
        if (unitCache == null)
        {
            // For future sprint
            Debug.Log("ERROR :: Unit Sprite Cache Needs to be defined -> LESelectionController.cs");
        }
        if (mapSpriteCache == null)
        {
            // For future sprint
            //Debug.Log("ERROR :: Map Sprite Cache Needs to be defined -> LESelectionController.cs");
        }
        ////////////////////////
        isTileMode = false;
        isInstanceMode = false;
        isSettingsMode = false;
    }

    public void SetSelectTile(int sprite, int variant)
    {
        isTileMode = true;
        isInstanceMode = false;
        isSettingsMode = false;
        selectedTileID = sprite;
        selectedTileVariantID = variant;
    }

    public void NullifyMode()
    {
        isInstanceMode = false;
        isSettingsMode = false;
        isTileMode = true;
    }

    public void SetSettingsType(LEUnitSettings settings)
    {
        isTileMode = false;
        isInstanceMode = false;
        isSettingsMode = true;
        selectedSettings = settings;
    }

    public void SetUnitType(LEUnitInstance unit)
    {
        isTileMode = false;
        isInstanceMode = true;
        isSettingsMode = false;
        selectedUnit = unit;
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
        return selectedUnitCacheID;
    }

    public int GetMapType()
    {
        // to be implemented next sprint
        return 0;
    }
}

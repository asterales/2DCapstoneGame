using UnityEngine;

public class LESelectionController : MonoBehaviour {
    // cache for tiles
    public LESpriteCache tileSpriteCache;
    public int selectedTileVariantID;
    public int selectedTileID;
    // cache for units
    public LEUnitCache unitCache;
    public int selectedUnitCacheID;
    // cache for depZones
    public LEDeploymentCache depCache;
    // cache for levels
    public LEMapCache mapCache;
    // currently selected unit info
    public LEUnitSettings selectedSettings;
    public LEUnitInstance selectedUnit;
    //public int selectedUnitID;
    public LEVictoryEditor victoryEditor;
    public LEUnitSettingsEditor unitEditor;
    public int selectedMapID;
    // reference to buttons
    public LEDeploymentButton depButton;
    public LETileButton tileButton;
    public LEUnitInstanceButton instanceButton;
    public LEUnitSettingsButton settingsButton;
    public LEVictoryConditionButton victoryConditionButton;

    public bool isTileMode;
    public bool isSettingsMode;
    public bool isInstanceMode;
    public bool isDepMode;
    public bool isVCMode;

    void Start()
    {
        selectedTileVariantID = 0;
        selectedTileID = 0;
        selectedUnitCacheID = 0;
        selectedMapID = 0;

        // for now this works, but eventually there will be 3 separate sprite caches
        tileSpriteCache = this.gameObject.GetComponent<LESpriteCache>();
        depCache = this.gameObject.GetComponent<LEDeploymentCache>();
        ////// DEBUG CODE //////
        if (tileSpriteCache == null)
        {
            Debug.Log("ERROR :: Tile Sprite Cache Needs to be defined -> LESelectionController.cs");
        }
        if (unitCache == null)
        {
            Debug.Log("ERROR :: Unit Sprite Cache Needs to be defined -> LESelectionController.cs");
        }
        if (depCache == null)
        {
            Debug.Log("ERROR :: Deployment Cache Needs to be defined -> LESelectionController.cs");
        }
        if (mapCache == null)
        {
            Debug.Log("ERROR :: Level Cache Needs to be defined -> LESelectionController.cs");
        }
        if (depButton == null)
        {
            Debug.Log("ERROR :: Reference to Deployment Button needs to be defined -> LESelectionController.cs");
        }
        if (instanceButton == null)
        {
            Debug.Log("ERROR :: Reference to Deployment Button needs to be defined -> LESelectionController.cs");
        }
        if (settingsButton == null)
        {
            Debug.Log("ERROR :: Reference to Settings Button needs to be defined -> LESelectionController.cs");
        }
        if (tileButton == null)
        {
            Debug.Log("ERROR :: Reference to Deployment Button needs to be defined -> LESelectionController.cs");
        }
        if (victoryEditor == null)
        {
            Debug.Log("ERROR :: Reference to VictoryEditor needs to be defined -> LESelectionController.cs");
        }
        ////////////////////////
        isTileMode = false;
        isInstanceMode = false;
        isSettingsMode = false;
        isDepMode = false;
        isVCMode = false;
        depButton.Deselect();
        tileButton.Deselect();
        instanceButton.Deselect();
        settingsButton.Deselect();
        //victoryConditionButton.Deselect();
    }

    public void SetSelectTile(int sprite, int variant)
    {
        isTileMode = true;
        isInstanceMode = false;
        isSettingsMode = false;
        isDepMode = false;
        isVCMode = false;
        tileButton.Select();
        selectedTileID = sprite;
        selectedTileVariantID = variant;
    }

    public void SetSelectTile()
    {
        isTileMode = true;
        isInstanceMode = false;
        isSettingsMode = false;
        isDepMode = false;
        isVCMode = false;
        tileButton.Select();
    }

    public void NullifyMode()
    {
        isInstanceMode = false;
        isSettingsMode = false;
        isDepMode = false;
        isTileMode = false;
        isVCMode = false;
        depButton.Deselect();
        instanceButton.Deselect();
        settingsButton.Deselect();
        victoryConditionButton.Deselect();
    }

    public void SetSettingsType(LEUnitSettings settings)
    {
        isTileMode = false;
        isInstanceMode = false;
        isDepMode = false;
        isSettingsMode = true;
        isVCMode = false;
        settingsButton.Select();
        selectedSettings = settings;
    }

    public void SetUnitType(LEUnitInstance unit)
    {
        isTileMode = false;
        isInstanceMode = true;
        isSettingsMode = false;
        isDepMode = false;
        isVCMode = false;
        instanceButton.Select();
        selectedUnit = unit;
    }

    public void SetUnitMode()
    {
        isTileMode = false;
        isSettingsMode = false;
        isInstanceMode = true;
        isDepMode = false;
        isVCMode = false;
        instanceButton.Select();
    }

    public void SetUnitSettingsMode()
    {
        if (selectedSettings != null)
        {
            isTileMode = false;
            isSettingsMode = true;
            isInstanceMode = false;
            isDepMode = false;
            isVCMode = false;
            settingsButton.Select();
        }
    }

    public void SetDepMode()
    {
        isTileMode = false;
        isInstanceMode = false;
        isSettingsMode = false;
        isDepMode = true;
        isVCMode = false;
        depButton.Select();
    }

    public void SetVCMode()
    {
        isTileMode = false;
        isInstanceMode = false;
        isSettingsMode = false;
        isDepMode = false;
        isVCMode = true;
        victoryConditionButton.Select();
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

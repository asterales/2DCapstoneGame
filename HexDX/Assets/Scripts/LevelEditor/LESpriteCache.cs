﻿using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LESpriteCache : MonoBehaviour {
    public List<Sprite> sprites;
    public List<LESpriteVariantCache> variantCaches;
    public GameObject tileBar;
    public CacheType cacheType;

    void Awake()
    {
        if (sprites == null) sprites = new List<Sprite>();
        variantCaches = new List<LESpriteVariantCache>();
        cacheType = CacheType.Tile;
        ////// DEBUG CODE //////
        if (tileBar == null)
        {
            Debug.Log("Error :: The Tile Bar Object For The Sprite Cache Needs to Be Defined -> LESpriteCache.cs");
        }
        ////////////////////////
        LoadInVariantsFromResources();
    }

    public Sprite GetTileSprite(int index)
    {
        int variantID = index % 100;
        int tileID = index / 100;
        return variantCaches[tileID].sprites[variantID];
    }

    public Sprite GetUnitSprite(int index)
    {
        // to be implemented next sprint
        return null;
    }

    public Sprite GetMapSprite(int index)
    {
        // to be implemented next sprint
        return null;
    }

    private void LoadInVariantsFromResources()
    {
        if (cacheType == CacheType.Tile) LoadInVariantsForTiles();
        else if (cacheType == CacheType.Unit) LoadInVariantsForUnits();
        else LoadInVariantsForMaps();
    }

    private void LoadInVariantsForTiles()
    {
        string path = "Assets\\Resources\\EditorSprites\\Tiles";
        string[] directories = Directory.GetDirectories(path);

        for (int i=0;i<directories.Length;i++)
        {
            // get all of the directories
            string[] files = Directory.GetFiles(directories[i]);
            // store all the sprites (ignore the meta files)
            string[] spriteFiles = new string[files.Length / 2];
            for(int j=0;j<files.Length;j+=2)
            {
                // have to cut off "Assets//Resources//" from the path as well as the extension
                spriteFiles[j / 2] = files[j].Substring(files[j].Length - (files[j].Length - 17));
                spriteFiles[j / 2] = spriteFiles[j / 2].Remove(spriteFiles[j / 2].IndexOf('.'));
            }

            // create a new variant cache
            variantCaches.Add(this.gameObject.AddComponent<LESpriteVariantCache>());
            // initialize the variant cache
            variantCaches[i].CreateCacheFromFiles(spriteFiles);
            variantCaches[i].id = i;
        }
    }

    private void LoadInVariantsForUnits()
    {
        string path = "Assets\\Resources\\EditorSprites\\Units";
        string[] directories = Directory.GetDirectories(path);
        for (int i = 0; i < directories.Length; i++)
        {
            // get all of the directories
            string[] files = Directory.GetFiles(directories[i]);
            // store all the sprites (ignore the meta files)
            string[] spriteFiles = new string[files.Length / 2];
            for (int j = 0; j < files.Length; j += 2)
            {
                // have to cut off "Assets//Resources//" from the path as well as the extension
                spriteFiles[j / 2] = files[j].Substring(files[j].Length - (files[j].Length - 17));
                spriteFiles[j / 2] = spriteFiles[j / 2].Remove(spriteFiles[j / 2].IndexOf('.'));
            }

            // create a new variant cache
            variantCaches.Add(this.gameObject.AddComponent<LESpriteVariantCache>());
            // initialize the variant cache
            variantCaches[i].CreateCacheFromFiles(spriteFiles);
        }
    }

    private void LoadInVariantsForMaps()
    {
        // to be implemented
    }
}

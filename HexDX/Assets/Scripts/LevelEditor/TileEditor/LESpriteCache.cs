﻿using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class LESpriteCache : MonoBehaviour {
    private static readonly string tileIdsFile = "TileIds";
    private static readonly string tileSpriteDir = "EditorSprites/Tiles/";
    public List<Sprite> sprites;
    public List<LESpriteVariantCache> variantCaches;
    public GameObject tileBar;

    void Awake()
    {
        if (sprites == null) sprites = new List<Sprite>();
        variantCaches = new List<LESpriteVariantCache>();
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

    private void LoadInVariantsFromResources()
    {
        LoadInVariantsForTiles();
    }

    private void LoadInVariantsForTiles()
    {
        string path = "Assets/Resources/EditorSprites/Tiles";

        string[] tileIdLines = Resources.Load<TextAsset>("EditorSprites/Tiles/" + tileIdsFile).text.Split('\n');

        for (int i=0; i< tileIdLines.Length; i++)
        {
            string[] lines = tileIdLines[i].Split(',');
            if (lines.Length != 2) continue;

            int id = Convert.ToInt32(lines[0]);
            string directory = lines[1];
            // get all of the files
            string[] files = Directory.GetFiles(path + "/" + directory.Trim() + "/");
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
            variantCaches[i].id = id;
        }
    }
}

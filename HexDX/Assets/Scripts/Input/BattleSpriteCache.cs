using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BattleSpriteCache : MonoBehaviour {
    private static readonly string tileIdsFile = "TileIds";
    private static readonly string tileSpriteDir = "EditorSprites/Tiles/";
    public List<BattleVariantCache> tileSprites;

	// Use this for initialization
	void Awake () {
        tileSprites = new List<BattleVariantCache>();
        //LoadSprites();
        LoadTileSprites();
	}

    private void LoadTileSprites() {
        TextAsset tileIds = Resources.Load<TextAsset>(tileSpriteDir + tileIdsFile);
        string[] tileIdLines = tileIds.text.Trim().Split('\n');
        for (int i = 0; i < tileIdLines.Length; i++) {
            string folderName = tileIdLines[i].Trim().Split(',')[1];
            tileSprites.Add(gameObject.AddComponent<BattleVariantCache>());
            tileSprites[i].sprites = Resources.LoadAll<Sprite>(tileSpriteDir + folderName).ToList();
        }
    }

    // private void LoadSprites()
    // {
    //     string path = "Assets/Resources/EditorSprites/Tiles";
    //     string[] directories = Directory.GetDirectories(path);
    //     for (int i = 0; i < directories.Length; i++)
    //     {
    //         // get all of the directories
    //         string[] files = Directory.GetFiles(directories[i]);
    //         // store all the sprites (ignore the meta files)
    //         string[] spriteFiles = new string[files.Length / 2];
    //         for (int j = 0; j < files.Length; j += 2)
    //         {
    //             // have to cut off "Assets//Resources//" from the path as well as the extension
    //             spriteFiles[j / 2] = files[j].Substring(files[j].Length - (files[j].Length - 17));
    //             spriteFiles[j / 2] = spriteFiles[j / 2].Remove(spriteFiles[j / 2].IndexOf('.'));
    //         }
    //         // create a new variant cache
    //         tileSprites.Add(this.gameObject.AddComponent<BattleVariantCache>());
    //         // initialize the variant cache
    //         tileSprites[i].CreateCacheFromFiles(spriteFiles);
    //     }
    // }

    public Sprite GetTileSprite(int type, int variant)
    {
        return tileSprites[type].sprites[variant];
    }
}

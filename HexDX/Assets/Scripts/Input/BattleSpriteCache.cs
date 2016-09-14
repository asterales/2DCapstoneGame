using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class BattleSpriteCache : MonoBehaviour {
    public List<BattleVariantCache> tileSprites;

	// Use this for initialization
	void Awake () {
        tileSprites = new List<BattleVariantCache>();
        LoadSprites();
	}

    private void LoadSprites()
    {
        string path = "Assets/Resources/EditorSprites/Tiles";
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
            tileSprites.Add(this.gameObject.AddComponent<BattleVariantCache>());
            // initialize the variant cache
            tileSprites[i].CreateCacheFromFiles(spriteFiles);
        }
    }

    public Sprite GetTileSprite(int type, int variant)
    {
        return tileSprites[type].sprites[variant];
    }
}

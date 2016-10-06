using UnityEngine;
using System.Collections.Generic;

public class BattleVariantCache : MonoBehaviour {
    public List<Sprite> sprites;

    void Awake()
    {
        sprites = new List<Sprite>();
    }

    // public void CreateCacheFromFiles(string[] spriteFiles)
    // {
    //     for (int i = 0; i < spriteFiles.Length; i++)
    //     {
    //         Sprite newSprite = Resources.Load<Sprite>(spriteFiles[i]);
    //         sprites.Add(newSprite);
    //     }
    // }
}

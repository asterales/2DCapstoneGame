using UnityEngine;
using System.Collections.Generic;

public class LESpriteVariantCache : MonoBehaviour {
    public List<Sprite> sprites;
    public int currentIndex;
    public int id;

	void Awake () {
        sprites = new List<Sprite>();
        currentIndex = 0;
        id = 0;
	}

    public void CreateCacheFromFiles(string[] spriteFiles)
    {
        for (int i=0;i<spriteFiles.Length;i++)
        {
            Sprite newSprite = Resources.Load<Sprite>(spriteFiles[i]);
            sprites.Add(newSprite);
        }
    }

    public Sprite GetCurrent()
    {
        return sprites[currentIndex];
    }
}

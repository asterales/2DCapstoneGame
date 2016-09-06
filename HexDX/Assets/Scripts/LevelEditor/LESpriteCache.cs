using UnityEngine;
using System.Collections.Generic;

public class LESpriteCache : MonoBehaviour {
    public List<Sprite> sprites;

    public Sprite GetSprite(int index)
    {
        return sprites[index];
    } 
}

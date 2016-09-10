using UnityEngine;
using System.Collections;

public class SpriteLoaderTest : MonoBehaviour {
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("EditorSprites\\Tiles\\Grass\\GrassSpace0");
    }
}

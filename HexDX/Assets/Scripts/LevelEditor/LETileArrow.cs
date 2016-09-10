using UnityEngine;
using System.Collections;

public class LETileArrow : MonoBehaviour {
    public LETileBar tileBar;
    public SpriteRenderer spriteRenderer;
    public bool isUpArrow;

    void Awake()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        ////// DEBUG CODE //////
        if (tileBar == null)
        {
            Debug.Log("ERROR :: Tile Bar needs to be Defined -> LETileArrow.cs");
        }
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: Sprite Renderer needs to be defined -> LETileArrow.cs");
        }
        ////////////////////////
    }

    void OnMouseDown()
    {
        spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f);
        if (isUpArrow) tileBar.MoveUp();
        else tileBar.MoveDown();
    }

    void OnMouseHover()
    {
        spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f);
    }

    void OnMouseEnter()
    {
        spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f);
    }

    void OnMouseUp()
    {
        spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f);
    }

    void OnMouseExit()
    {
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
    }
}

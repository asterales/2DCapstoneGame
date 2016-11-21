using UnityEngine;

public class LELevelScrollButton : MonoBehaviour {
    public LELevelScroll reference;
    public SpriteRenderer spriteRenderer;
    public bool isRight;

    void Awake()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        ////// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: Need Reference to Sprite Renderer -> LELevelScrollButton.cs");
        }
        if (reference == null)
        {
            Debug.Log("ERROR :: Need Reference to Level Scroll -> LELevelScrollButton.cs");
        }
        ////////////////////////
    }

    void OnMouseDown()
    {
        if (isRight) reference.ScrollRight();
        else reference.ScrollLeft();
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
        spriteRenderer.color = Color.white;
    }
}

using UnityEngine;

public class LEHorizonArrow : MonoBehaviour {
    public LEHorizonBar tileBar;
    public SpriteRenderer spriteRenderer;
    public bool isRightArrow;

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

    public void TurnOn()
    {
        Color color = spriteRenderer.color;
        spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f);
        Vector3 currentPos = this.transform.position;
        this.transform.position = new Vector3(currentPos.x, currentPos.y, -1.0f);
    }

    public void TurnOff()
    {
        Color color = spriteRenderer.color;
        spriteRenderer.color = new Color(color.r, color.g, color.b, 0.0f);
        Vector3 currentPos = this.transform.position;
        this.transform.position = new Vector3(currentPos.x, currentPos.y, 4.0f);
    }

    void OnMouseDown()
    {
        spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f, spriteRenderer.color.a);
        if (spriteRenderer.color.a > 0.0f)
        {
            if (isRightArrow) tileBar.MoveRight();
            else tileBar.MoveLeft();
        }
    }

    void OnMouseHover()
    {
        spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f, spriteRenderer.color.a);
    }

    void OnMouseEnter()
    {
        spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f, spriteRenderer.color.a);
    }

    void OnMouseUp()
    {
        spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f, spriteRenderer.color.a);
    }

    void OnMouseExit()
    {
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, spriteRenderer.color.a);
    }
}

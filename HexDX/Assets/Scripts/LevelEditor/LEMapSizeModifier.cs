using UnityEngine;
using System.Collections;

public class LEMapSizeModifier : MonoBehaviour {
    public LEHexMap hexMap;
    public bool isIncrease;
    public bool isVertical;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        ///// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: Sprite Renderer needs to be defined -> LEMapSizeModifier.cs");
        }
        if (hexMap == null)
        {
            Debug.Log("ERROR :: HexMap needs to be defined -> LEMapSizeModifier.cs");
        }
        ///////////////////////
    }

    void OnMouseDown()
    {
        spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f);
        if (isIncrease && isVertical) hexMap.IncrementVert();
        if (isIncrease && !isVertical) hexMap.IncrementHori();
        if (!isIncrease && isVertical) hexMap.DecrementVert();
        if (!isIncrease && !isVertical) hexMap.DecrementHori();
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

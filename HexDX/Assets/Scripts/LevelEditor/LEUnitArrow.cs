using UnityEngine;
using System.Collections;

public class LEUnitArrow : MonoBehaviour {
    public LEUnitBar unitBar;
    public SpriteRenderer spriteRenderer;
    public bool isUpArrow;

    void Awake()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        ////// DEBUG CODE //////
        if (unitBar == null)
        {
            Debug.Log("ERROR :: Need reference to UnitBar object -> LEUnitArrow.cs");
        }
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: Reference to SpriteRenderer is not defined -> LEUnitArrow.cs");
        }
        ////////////////////////
    }
    void OnMouseDown()
    {
        spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f);
        if (isUpArrow) unitBar.MoveUp();
        else unitBar.MoveDown();
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

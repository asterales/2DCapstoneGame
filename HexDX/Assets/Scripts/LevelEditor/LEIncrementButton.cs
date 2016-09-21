using UnityEngine;
using System.Collections;

public class LEIncrementButton : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public LEStatEditor parent;
    public int modifier;
    public bool isOn;

	void Awake () {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
	    ////// DEBUG CODE //////
        if (parent == null)
        {
            Debug.Log("ERROR :: Reference to StatEditor needs to be defined -> LEIncrementButton.cs");
        }
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: SpriteRenderere needs to be defined -> LEIncrementButton.cs");
        }
        ////////////////////////
        isOn = false;
        TurnOff();
	}

    public void TurnOn()
    {
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        isOn = true;
    }

    public void TurnOff()
    {
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        isOn = false;
    }

    void OnMouseDown()
    {
        if (isOn)
        {
            spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f);
            parent.ChangeState(modifier);
        }
    }

    void OnMouseHover()
    {
        if (isOn)
        {
            spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f);
        }
    }

    void OnMouseEnter()
    {
        if (isOn)
        {
            spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f);
        }
    }

    void OnMouseUp()
    {
        if (isOn)
        {
            spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f);
        }
    }

    void OnMouseExit()
    {
        if (isOn)
        {
            spriteRenderer.color = Color.white;
        }
    }
}

using UnityEngine;

public class LEEditorIncrementButton : MonoBehaviour {
    public LEEditor reference;
    public SpriteRenderer spriteRenderer;
    public int modifier;
    public bool isOn;

    void Start () {
        ////// DEBUG CODE //////
	    if (reference == null)
        {
            Debug.Log("ERROR :: Need Reference to Editor Object -> LEEditorIncrementButton.cs");
        }
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: Need Reference to the Sprite Renderer -> LEEditorIncrementButton.cs");
        }
        ////////////////////////
	}

    public void TurnOn()
    {
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        isOn = true;
        Vector3 pos = this.transform.localPosition;
        this.transform.localPosition = new Vector3(pos.x, pos.y, 0.0f);
    }

    public void TurnOff()
    {
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        isOn = false;
        Vector3 pos = this.transform.localPosition;
        this.transform.localPosition = new Vector3(pos.x, pos.y, -10.0f);
    }

    void OnMouseDown()
    {
        spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f);
        if (modifier > 0) reference.Increment(modifier);
        else reference.Decrement(modifier);
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

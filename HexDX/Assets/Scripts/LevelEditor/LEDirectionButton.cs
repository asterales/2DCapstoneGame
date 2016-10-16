using UnityEngine;

public class LEDirectionButton : MonoBehaviour {
    public LEDirectionEditor parent;
    private SpriteRenderer spriteRenderer;
    public int direction;

	void Start () {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        ////// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("ERROR :: Sprite Renderer needs to be defined");
        }
        ////////////////////////
	}

    public void TurnOff()
    {
        spriteRenderer.color = Color.white;
    }

    public void TurnOn()
    {
        spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f);
    }

    public void Activate()
    {
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        // move it
    }

    public void DeActivate()
    {
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        // move it
    }

    void OnMouseDown()
    {
        parent.UpdateDirection(direction);
    }
}

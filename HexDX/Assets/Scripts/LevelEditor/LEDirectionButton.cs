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
        DeActivate();
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
        Vector3 pos = this.transform.localPosition;
        this.transform.localPosition = new Vector3(pos.x, pos.y, 0.0f);
    }

    public void DeActivate()
    {
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Vector3 pos = this.transform.localPosition;
        this.transform.localPosition = new Vector3(pos.x, pos.y, -10.0f);
    }

    void OnMouseDown()
    {
        parent.UpdateDirection(direction);
    }
}

using UnityEngine;

public class LEHorizonBarButton : MonoBehaviour {
    public GameObject background;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        ///// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("Error :: Sprite Renderer needs to be defined -> LEHorizonBarButton.cs");
        }
        if (background == null)
        {
            Debug.Log("ERROR :: Background Reference needs to be defined -> LEHorizonBarButton.cs");
        }
        ///////////////////////
    }

    void OnMouseDown()
    {
        //Debug.Log("TOUCHED");
        //TurnOff();
        // to be implemneted
    }

    public void SetAvatar(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void TurnOn()
    {
        SpriteRenderer backgroundRenderer = background.GetComponent<SpriteRenderer>();
        Color color = backgroundRenderer.color;
        backgroundRenderer.color = new Color(color.r, color.g, color.b, 1.0f);

        color = spriteRenderer.color;
        spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f);
        Vector3 currentPos = this.transform.position;
        this.transform.position = new Vector3(currentPos.x, currentPos.y, -1.0f);
    }

    public void TurnOff()
    {
        SpriteRenderer backgroundRenderer = background.GetComponent<SpriteRenderer>();
        Color color = backgroundRenderer.color;
        backgroundRenderer.color = new Color(color.r, color.g, color.b, 0.0f);

        color = spriteRenderer.color;
        spriteRenderer.color = new Color(color.r, color.g, color.b, 0.0f);
        Vector3 currentPos = this.transform.position;
        this.transform.position = new Vector3(currentPos.x, currentPos.y, 4.0f);
    }
}

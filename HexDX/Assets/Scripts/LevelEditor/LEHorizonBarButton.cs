using UnityEngine;

public class LEHorizonBarButton : MonoBehaviour {
    public GameObject background;
    public SpriteRenderer spriteRenderer;
    public LESpriteVariantCache spriteCache;
    public LEHorizonBar parentBar;
    public int currentIndex;

    void Awake()
    {
        currentIndex = -1;
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        parentBar = this.gameObject.GetComponentInParent<LEHorizonBar>();
        // temporary
        //spriteRenderer.sprite = Resources.Load<Sprite>("EditorSprites\\Tiles\\Grass\\GrassSpace0");
        ///// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("Error :: Sprite Renderer needs to be defined -> LEHorizonBarButton.cs");
        }
        if (background == null)
        {
            Debug.Log("ERROR :: Background Reference needs to be defined -> LEHorizonBarButton.cs");
        }
        if (parentBar == null)
        {
            Debug.Log("ERROR :: Need Reference to Parent Bar object");
        }
        ///////////////////////
    }

    void OnMouseDown()
    {
        if (spriteCache != null)
        {
            spriteCache.currentIndex = currentIndex;
            parentBar.OnMakeChoice();
        }
    }

    public void SetAvatar(LESpriteVariantCache sprite, int index)
    {
        spriteCache = sprite;
        spriteRenderer.sprite = spriteCache.sprites[index];
        currentIndex = index;
    }

    public void TurnOn()
    {
        //Debug.Log("adfasdf");
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
        currentIndex = -1;
    }
}

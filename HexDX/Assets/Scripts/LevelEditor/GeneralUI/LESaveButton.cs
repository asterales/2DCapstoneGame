using UnityEngine;

public class LESaveButton : MonoBehaviour {
    public LEMapWriter mapWriter;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f);
        mapWriter.WriteLevel();
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

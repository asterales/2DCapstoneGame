using UnityEngine;

public class LEHorizonBarButton : MonoBehaviour {
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        ///// DEBUG CODE //////
        if (spriteRenderer == null)
        {
            Debug.Log("Error :: Sprite Renderer needs to be defined -> LEHorizonBarButton.cs");
        }
        ///////////////////////
    }

    public void SetAvatar(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}

using UnityEngine;
using System.Collections;

public class LEIncrementButton : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public LEStatEditor statEditorParent;
    public LEAIStatEditor aiEditorParent;
    public int modifier;
    public bool isOn;

	void Awake () {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
	    ////// DEBUG CODE //////
        if (statEditorParent == null || aiEditorParent == null)
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
        if (statEditorParent) statEditorParent.ChangeState(modifier);
        if (aiEditorParent) aiEditorParent.ChangeState(modifier);
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

using UnityEngine;
using System.Collections;

public class LESelectHack : MonoBehaviour {
    public SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        Vector4 currentColor = spriteRenderer.color;
        spriteRenderer.color = new Vector4(currentColor.x, currentColor.y, currentColor.z, 0.2f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

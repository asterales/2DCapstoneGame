using UnityEngine;

public class UnitDeath : MonoBehaviour {
    private SpriteRenderer spriteRenderer;
    private float fadeToRed =.166f;
    private float wait = .333f;
    private float fadeOut = .8333f;
	
    // Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<Unit>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (fadeToRed>0) {
            fadeToRed -= Time.deltaTime;
            spriteRenderer.color = new Color(1, fadeToRed/.166f, fadeToRed/.166f, 1);
            return;
        }
        if (wait > 0) {
            wait -= Time.deltaTime;
            return;
        }
        if (fadeOut > 0) {
            fadeOut -= Time.deltaTime;
            spriteRenderer.color = new Color(1, 0, 0, fadeOut/.8333f);
            return;
        }
        gameObject.SetActive(false);
        GameObject.Destroy(this);
    }
}

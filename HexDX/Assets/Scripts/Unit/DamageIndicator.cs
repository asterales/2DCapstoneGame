using UnityEngine;

public class DamageIndicator : MonoBehaviour {
    public TextMesh textmesh;
    public MeshRenderer meshrenderer;
	
    // Use this for initialization
	void Awake () {
        meshrenderer = gameObject.AddComponent<MeshRenderer>();
        meshrenderer.sortingOrder = 3;
        textmesh = gameObject.AddComponent<TextMesh>();
        transform.localScale = Camera.main.transform.localScale;
        textmesh.fontSize = 8;
        textmesh.alignment = TextAlignment.Center;
        textmesh.anchor = TextAnchor.LowerCenter;
	}
	
    public void SetDamage(int damage) {
        textmesh.text = "" + damage;
        if (damage < 0) {
            textmesh.color = Color.red;
        } else {
            textmesh.color = Color.green;
        }
    }

    public void SetDamage(string damage) {
        textmesh.text = damage;
        textmesh.color = Color.red;
    }

	// Update is called once per frame
	void Update () {
        textmesh.color = textmesh.color - new Color(0, 0, 0, .01f);
        transform.localScale = Camera.main.transform.localScale;
        if (textmesh.color.a <= 0) {
            GameObject.Destroy(gameObject);
            return;
        }
        transform.position+=new Vector3(0, .02f, 0);
	}
}

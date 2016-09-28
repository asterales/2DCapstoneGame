using UnityEngine;
using System.Collections;

public class DamageIndicator : MonoBehaviour {
    public TextMesh textmesh;
    public MeshRenderer meshrenderer;
	// Use this for initialization
	void Awake () {
        meshrenderer = this.gameObject.AddComponent<MeshRenderer>();
        meshrenderer.sortingOrder = 3;
        textmesh = this.gameObject.AddComponent<TextMesh>();
        textmesh.fontSize = 15;
        textmesh.text = "potato";
        textmesh.alignment = TextAlignment.Center;
        textmesh.anchor = TextAnchor.LowerCenter;
	}
	
    public void SetDamage(int damage)
    {
        textmesh.text = "" + damage;
        if (damage < 0)
            textmesh.color = Color.red;
        else
            textmesh.color = Color.green;

    }

    public void SetDamage(string damage)
    {
        textmesh.text = damage;
        textmesh.color = Color.red;
    }
	// Update is called once per frame
	void Update () {
        textmesh.color = textmesh.color - new Color(0, 0, 0, .01f);
        if (textmesh.color.a <= 0)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        transform.position+=new Vector3(0, .04f, 0);
	}
}

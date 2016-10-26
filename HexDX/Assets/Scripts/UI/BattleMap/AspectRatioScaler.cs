using UnityEngine;
using System.Collections;

public class AspectRatioScaler : MonoBehaviour {
    float originalratio = 16.0f / 9.0f;
    float currentAspect;
    float origx, origy;

	// Use this for initialization
	void Start () {
        float ratio = Camera.main.aspect;
        origx = transform.localScale.x;
        origy = transform.localScale.y;
        currentAspect = ratio;
        transform.localScale = new Vector3(origx*ratio/originalratio, origy, 1);
    }
	
	// Update is called once per frame
	void Update () {
        if (Camera.main.aspect != currentAspect)
        {
            float ratio = Camera.main.aspect;
            currentAspect = ratio;
            transform.localScale = new Vector3(origx * ratio / originalratio, origy, 1);
        }
    }
}

using UnityEngine;
using System.Collections;

public class SpeedController : MonoBehaviour {

    public static float speed = 1.0f;

	// Use this for initialization
	void Start () {
        speed = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space)){
            speed = 4.0f;
        }
        else
        {
            speed = 1.0f;
        }
	
	}
}

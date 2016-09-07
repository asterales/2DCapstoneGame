using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float  newX=0,newY = 0;
        if (Input.GetKey(KeyCode.W))
            newY += .1f;
        if (Input.GetKey(KeyCode.S))
            newY -= .1f;
        if (Input.GetKey(KeyCode.A))
            newX -= .1f;
        if (Input.GetKey(KeyCode.D))
            newX += .1f;
        transform.position += new Vector3(newX, newY, 0);

    }
}

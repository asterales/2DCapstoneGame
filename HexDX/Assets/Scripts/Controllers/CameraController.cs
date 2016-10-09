using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    // Use this for initialization
    void Start () {
    }
    
    // Update is called once per frame
    void Update () {
        float newX, newY;
        newX = newY = 0;
        if (Input.GetKey(KeyCode.W))
            newY += .2f * transform.localScale.y;
        if (Input.GetKey(KeyCode.S))
            newY -= .2f * transform.localScale.y;
        if (Input.GetKey(KeyCode.A))
            newX -= .2f * transform.localScale.x;
        if (Input.GetKey(KeyCode.D))
            newX += .2f * transform.localScale.x;

        transform.position += new Vector3(newX, newY, 0) * 2.0f;
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
        {
            Camera.main.orthographicSize *= 1.1f;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * (Camera.main.orthographicSize / (24));

        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // back
        {
            Camera.main.orthographicSize *= .9f;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * (Camera.main.orthographicSize / (24));
        }
    }
}

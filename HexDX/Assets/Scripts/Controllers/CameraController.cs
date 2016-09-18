using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public static Camera camera;

    // Use this for initialization
    void Start () {
        camera = gameObject.GetComponent<Camera>();
    }
    
    // Update is called once per frame
    void Update () {
        float newX, newY;
        newX = newY = 0;
        if (Input.GetKey(KeyCode.W))
            newY += .1f;
        if (Input.GetKey(KeyCode.S))
            newY -= .1f;
        if (Input.GetKey(KeyCode.A))
            newX -= .1f;
        if (Input.GetKey(KeyCode.D))
            newX += .1f;

        transform.position += new Vector3(newX, newY, 0) * 2.0f;
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
        {
            camera.orthographicSize++;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * (camera.orthographicSize / (24));

        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // back
        {
            if (camera.orthographicSize > 1)
            {
                camera.orthographicSize--;
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * (camera.orthographicSize / (24));
            }
        }
    }
}

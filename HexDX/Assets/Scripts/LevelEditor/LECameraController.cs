using UnityEngine;
using System.Collections;

public class LECameraController : MonoBehaviour
{
    public static Camera myCamera;

    // Use this for initialization
    void Start()
    {
        myCamera = gameObject.GetComponent<Camera>();
        LEHexMap hexmap = GameObject.Find("LEController").GetComponent<LEHexMap>();
        transform.position = hexmap.tileArray[hexmap.tileArray.Count / 2][hexmap.tileArray[0].Count / 2].transform.position - new Vector3(0, 0, 100) ;
    }

    // Update is called once per frame
    void Update()
    {
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
        if (Input.GetAxis("Mouse ScrollWheel")< 0) // forward
        {
                Camera.main.orthographicSize++;
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * (Camera.main.orthographicSize / (30));

        }
        if (Input.GetAxis("Mouse ScrollWheel")>0) // back
        {
            if (Camera.main.orthographicSize > 1)
            {
                Camera.main.orthographicSize--;
                transform.localScale=new Vector3(1.0f, 1.0f, 1.0f) * (Camera.main.orthographicSize / (30));
            }
        }
    }
}

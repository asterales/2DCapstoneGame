using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    private float originalSize, xmin, xmax, ymin, ymax;
    // Use this for initialization
    void Start () {
        originalSize = Camera.main.orthographicSize;
        int hexmapH = HexMap.mapArray.Count;
        int hexmapW = HexMap.mapArray[0].Count;
        xmin = HexMap.mapArray[0][0].transform.position.x;
        ymax = HexMap.mapArray[0][0].transform.position.y;
        xmax = HexMap.mapArray[hexmapH-1][hexmapW-1].transform.position.x;
        ymin = HexMap.mapArray[hexmapH-1][hexmapW-1].transform.position.y;

    }
    
    // Update is called once per frame
    void Update () {
        float newX, newY;
        newX = newY = 0;
        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y > Screen.height * .9f) 
            newY += .2f * transform.localScale.y;
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y < Screen.height * .1f)
            newY -= .2f * transform.localScale.y;
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x < Screen.width * .1f)
            newX -= .2f * transform.localScale.x;
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x > Screen.width * .9f)
            newX += .2f * transform.localScale.x;
        transform.position += new Vector3(newX, newY, 0) * 2.0f;
        if (transform.position.x < xmin)
            transform.position = new Vector3(xmin, transform.position.y, transform.position.z);
        else if (transform.position.x > xmax)
            transform.position = new Vector3(xmax, transform.position.y, transform.position.z);
        if (transform.position.y < ymin)
            transform.position = new Vector3(transform.position.x, ymin, transform.position.z);
        else if (transform.position.y > ymax)
            transform.position = new Vector3(transform.position.x, ymax, transform.position.z);
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
        {
            Camera.main.orthographicSize *= 1.1f;
            Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize, 20.0f);
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * (Camera.main.orthographicSize / originalSize);

        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // back
        {
            Camera.main.orthographicSize *= .9f;
            Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, 2.0f);
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * (Camera.main.orthographicSize / originalSize);
        }

    }
}

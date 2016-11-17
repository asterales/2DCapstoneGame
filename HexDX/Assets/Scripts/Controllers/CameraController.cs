using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CameraController : MonoBehaviour {
    private const float MOUSE_MOVE_MARGIN = 0.1f;
    private const float DECREASE_ZOOM_FACTOR = 0.9f;
    private const float INCREASE_ZOOM_FACTOR = 1.1f;
    private const float MIN_ZOOM = 2.0f;
    private const float MAX_ZOOM = 20.0f;

    private float originalSize, xmin, xmax, ymin, ymax;
    private Vector3 prev;
    
    private bool aiMovedCamera;

    void Awake() {
        originalSize = Camera.main.orthographicSize;
    }

    // Use this for initialization
    public void InitCamera (List<Vector3> positions = null) {
        SetBounds();
        if (positions != null && positions.Count > 0) {
            CenterCamera(positions);
        }
    }

    private void SetBounds() {
        int hexmapH = HexMap.mapArray.Count;
        int hexmapW = HexMap.mapArray[0].Count;
        xmin = HexMap.mapArray[0][0].transform.position.x;
        ymax = HexMap.mapArray[0][0].transform.position.y;
        xmax = HexMap.mapArray[hexmapH-1][hexmapW-1].transform.position.x;
        ymin = HexMap.mapArray[hexmapH-1][hexmapW-1].transform.position.y;
    }

    private void CenterCamera(List<Vector3> positions) {
        Vector2 avgPos = new Vector2(positions.Average(p => p.x), positions.Average(p => p.y));
        ClampCameraPosition(avgPos);
    }
    
    // Update is called once per frame
    void Update () {
        if (BattleController.instance.IsPlayerTurn) {
            // If player turn just started and need to pan back to original position
            if (aiMovedCamera) {
                MoveCameraTowards(prev);
                if (transform.position == prev) {
                    aiMovedCamera = false;
                }
            } else {
                prev = transform.position;
                ClampCameraPosition(transform.position + GetPanVector());
            }
        } else {
            Unit aiUnit = BattleControllerManager.instance.ai.GetUnit();
            if (aiUnit) {
                aiMovedCamera = true;
                MoveCameraTowards(aiUnit.transform.position);
            }
        }
        SetZoomLevel();
    }

    private Vector3 GetPanVector() {
        float newX, newY;
        newX = newY = 0;
        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y > Screen.height * (1 - MOUSE_MOVE_MARGIN)) {
            newY += .2f * transform.localScale.y;
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y < Screen.height * MOUSE_MOVE_MARGIN) {
            newY -= .2f * transform.localScale.y;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x < Screen.width * MOUSE_MOVE_MARGIN) {
            newX -= .2f * transform.localScale.x;
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x > Screen.width * (1 - MOUSE_MOVE_MARGIN)) {
            newX += .2f * transform.localScale.x;
        }
        return new Vector3(newX, newY, 0) * 2.0f;
    }

    private void SetZoomLevel() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) { // back
            Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize * DECREASE_ZOOM_FACTOR, MIN_ZOOM);
            transform.localScale = Vector3.one * (Camera.main.orthographicSize / originalSize);
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0) { //forward
            Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize * INCREASE_ZOOM_FACTOR, MAX_ZOOM);
            transform.localScale = Vector3.one * (Camera.main.orthographicSize / originalSize);
        }
    }

    private void ClampCameraPosition(Vector2 pos) {
        transform.position = new Vector3(Mathf.Clamp(pos.x, xmin, xmax), Mathf.Clamp(pos.y, ymin, ymax), transform.position.z);
    }

    private void MoveCameraTowards(Vector3 pos) {
        Vector3 destination = new Vector3(pos.x, pos.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, destination, transform.localScale.x);
    }

    public void DontMove() {
        if (BattleController.instance.IsPlayerTurn) {
            transform.position = prev;
        }
    }
}

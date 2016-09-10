using UnityEngine;
using System.Collections;

public class SelectionController : MonoBehaviour {
    public static Tile selectedTile;
    public Sprite selectedSprite;
    public static Unit selectedUnit;
    private GameObject selectedSpace; // object for selected space
    private static readonly Vector3 visibilityOffset = new Vector3(0, 0, -0.01f);

	// Use this for initialization
	void Start () {
        selectedTile = null;

        // create selected space
        selectedSpace = new GameObject(string.Format("Selected Space"));
        selectedSpace.AddComponent<SpriteRenderer>();
        selectedSpace.transform.position = new Vector3(-1000, -1000, 0);
        selectedSpace.GetComponent<SpriteRenderer>().sprite = selectedSprite;
        selectedSpace.GetComponent<SpriteRenderer>().sortingOrder = 2;
        selectedSpace.GetComponent<SpriteRenderer>().color = new Vector4(1.0f, 1.0f, 1.0f, .4f);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1)) {
            ClearSelection();
            HexMap.ClearMovementTiles();
        }
        if (selectedTile != null) {
            // update position of selected space
            selectedSpace.transform.position = selectedTile.transform.position + visibilityOffset;
            if (selectedTile.currentUnit) {
                selectedUnit = selectedTile.currentUnit;
            }
        }
	}

    public void ClearSelection() {
        selectedTile = null;
        selectedSpace.transform.position = new Vector3(-1000, -1000, 0);
    }
}

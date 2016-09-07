using UnityEngine;
using System.Collections;

public class SelectionController : MonoBehaviour {
    public Tile selectedTile;
    public Tile clickedTile; // tile currently clicked
    public Sprite selectedSprite;

    private GameObject selectedSpace; // object for selected space

	// Use this for initialization
	void Start () {
        selectedTile = null;
        clickedTile = null;

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
        if (Input.GetMouseButtonDown(1))
        {
            ClearSelection();
            HexMap.ClearMovementTiles();
        }
	    if (clickedTile != null)
        {
            selectedTile = clickedTile;
            // do stuff to the clicked tile
        }
        if (selectedTile != null)
        {
            // update position of selected space
            Vector3 pos = selectedTile.transform.position;
            selectedSpace.transform.position = new Vector3(pos.x, pos.y, -0.001f);
        }
	}

    public void ClearSelection() {
        selectedTile = null;
        clickedTile = null;
        selectedSpace.transform.position = new Vector3(-1000, -1000, 0);
    }
}

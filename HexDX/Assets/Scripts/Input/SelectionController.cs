using UnityEngine;
using System.Collections;

public class SelectionController : MonoBehaviour {
    public static Tile selectedTile;
    public Sprite selectedSprite;
    public static Unit selectedUnit;
    public static SelectionMode selectionMode;
    private static GameObject selectedSpaceObj; // object for selected space
    private static readonly Vector3 visibilityOffset = new Vector3(0, 0, -0.01f);

	void Start () {
        selectedTile = null;
        InitSelectSpaceObj();
    }
	
    private void InitSelectSpaceObj() {
        selectedSpaceObj = new GameObject(string.Format("Selected Space"));
        selectedSpaceObj.AddComponent<SpriteRenderer>();
        selectedSpaceObj.transform.position = new Vector3(-1000, -1000, 0);
        selectedSpaceObj.GetComponent<SpriteRenderer>().sprite = selectedSprite;
        selectedSpaceObj.GetComponent<SpriteRenderer>().sortingOrder = 2;
        selectedSpaceObj.GetComponent<SpriteRenderer>().color = new Vector4(1.0f, 1.0f, 1.0f, .4f);
    }

	void Update () {
        if (SelectionController.TakingInput()) {
            if (selectedTile != null) {
                DisplayTileIsSelected();
                if (selectedTile.currentUnit) {
                    selectedUnit = selectedTile.currentUnit;
                }
            }
        }
	}

    private void DisplayTileIsSelected() {
        selectedSpaceObj.transform.position = selectedTile.transform.position + visibilityOffset;
    }

    public static void ClearSelection() {
        selectedTile = null;
        selectedSpaceObj.transform.position = new Vector3(-1000, -1000, 0);
    }

    // if the game is currently taking input
    // used to make sure the user cant click while moving a unit
    public static bool TakingInput()
    {
        return selectionMode == SelectionMode.Open;
    }

    public static bool TakingAIInput()
    {
        return selectionMode == SelectionMode.AITurn;
    }
}

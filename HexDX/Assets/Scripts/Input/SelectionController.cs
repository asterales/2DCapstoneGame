using UnityEngine;
using System.Collections;

public class SelectionController : MonoBehaviour {
    public static Tile selectedTile;
    public Sprite selectedSprite;
    public RuntimeAnimatorController animation;
    public static Unit selectedUnit;
    public static Unit target;
    public static SelectionMode selectionMode;
    private static GameObject selectedSpaceObj; // object for selected space
    private static GameObject targetSpaceObj; // object for target space
    private static readonly Vector3 visibilityOffset = new Vector3(0, 0, -0.01f);

	void Start () {
        selectedTile = null;
        InitSelectSpaceObj();
    }
	
    private void InitSelectSpaceObj() {
        selectedSpaceObj = new GameObject(string.Format("Selected Space"));
        SpriteRenderer sr = selectedSpaceObj.AddComponent<SpriteRenderer>();
        sr.sprite = selectedSprite;
        sr.sortingOrder = 2;
        sr.color = Color.black;
        Animator animator = selectedSpaceObj.AddComponent<Animator>();
        animator.runtimeAnimatorController = animation;
        selectedSpaceObj.transform.position = new Vector3(-1000, -1000, 0);

        targetSpaceObj = new GameObject(string.Format("Target Space"));
        sr = targetSpaceObj.AddComponent<SpriteRenderer>();
        sr.sprite = selectedSprite;
        sr.sortingOrder = 2;
        sr.color = Color.white;
        animator = targetSpaceObj.AddComponent<Animator>();
        animator.runtimeAnimatorController = animation;
        targetSpaceObj.transform.position = new Vector3(-1000, -1000, 0);
    }

	void Update () {
        if (selectedTile != null)
        {
            DisplayTileIsSelected();
            if (selectedTile.currentUnit)
            {
                selectedUnit = selectedTile.currentUnit;
            }
        }
        else if (selectedUnit != null)
        {
            DisplayUnitIsSelected();
        }


        if (SelectionController.selectionMode == SelectionMode.Attacking)
        {
            DisplayTargetIsSelected();
        }
        else
        {
            targetSpaceObj.transform.position = new Vector3(-1000, -1000, 0);
        }
	}

    private void DisplayTileIsSelected() {
        selectedSpaceObj.transform.position = selectedTile.transform.position + visibilityOffset;
    }

    private void DisplayUnitIsSelected()
    {
        selectedSpaceObj.transform.position = selectedUnit.transform.position + visibilityOffset;
    }

    private void DisplayTargetIsSelected()
    {
        targetSpaceObj.transform.position = target.currentTile.transform.position + visibilityOffset;
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

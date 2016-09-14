using UnityEngine;
using System.Collections;

public class SelectionController : MonoBehaviour {
    public static Unit selectedUnit { get { return selectedTile ? selectedTile.currentUnit : null; } }
    public static Tile selectedTile;
    public Sprite selectedSprite;
    public RuntimeAnimatorController animation;
    public static Unit target;
    private static SelectionMode selectionMode;
    private static GameObject selectedSpaceObj; // object for selected space
    private static GameObject targetSpaceObj; // object for target space
    private static readonly Vector3 visibilityOffset = new Vector3(0, 0, -0.01f);

	void Start () {
        selectedTile = null;
        InitSelectSpaceObj();
        InitTargetSpaceObj();
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
    }

    private void InitTargetSpaceObj() {
        targetSpaceObj = new GameObject(string.Format("Target Space"));
        SpriteRenderer sr = targetSpaceObj.AddComponent<SpriteRenderer>();
        sr.sprite = selectedSprite;
        sr.sortingOrder = 2;
        sr.color = Color.white;
        Animator animator = targetSpaceObj.AddComponent<Animator>();
        animator.runtimeAnimatorController = animation;
        targetSpaceObj.transform.position = new Vector3(-1000, -1000, 0);
    }

    public static void SetSelectedTile(Tile tile) {
        selectedTile = tile;
        DisplayTileIsSelected();
        DisplayUnitIsSelected();
        //Display stats
    }

    public static void SetSelectedTarget(Unit unit) {
        target = unit;
        DisplayTargetIsSelected();
    }

    private static void DisplayTileIsSelected() {
        selectedSpaceObj.transform.position = selectedTile.transform.position + visibilityOffset;
    }

    private static void DisplayUnitIsSelected() {
        if(selectedUnit){
            selectedSpaceObj.transform.position = selectedUnit.transform.position + visibilityOffset;
        }
    }

    private static void DisplayTargetIsSelected() {
        if (target) {
            targetSpaceObj.transform.position = target.currentTile.transform.position + visibilityOffset;
        } else {
            targetSpaceObj.transform.position = new Vector3(-1000, -1000, 0);
        }
    }

    public static void ClearSelection() {
        selectedTile = null;
        target = null;
        selectedSpaceObj.transform.position = new Vector3(-1000, -1000, 0);
        targetSpaceObj.transform.position = new Vector3(-1000, -1000, 0);
    }

    public static void OpenTileSelection() {
        selectionMode = SelectionMode.Open;
    }

    public static void RestrictToAttackTiles() {
        selectionMode = SelectionMode.Attacking;
    }

    public static void DisableTileSelection() {
        selectionMode = SelectionMode.Disabled;
    }

    public static bool IsMode(SelectionMode mode) {
        return selectionMode == mode;
    }
}

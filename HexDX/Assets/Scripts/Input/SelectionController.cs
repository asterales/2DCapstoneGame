using UnityEngine;
using System.Collections.Generic;

public class SelectionController : MonoBehaviour {
    public Sprite selectedSprite;
    public RuntimeAnimatorController animation;
    public static Unit selectedUnit;
    public static Tile selectedTile;
    public static Unit target;
    private static Dictionary<Unit, Tile> lastTiles = new Dictionary<Unit, Tile>();
    public static SelectionMode mode;
    private static GameObject selectedSpaceObj; // object for selected space
    private static GameObject targetSpaceObj; // object for target space

    void Awake() {
        mode = SelectionMode.Open;
    }

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


    protected virtual void Update () {
        if(!SelectionController.TakingAIInput()) {
            if (mode == SelectionMode.Open) {
                if (selectedTile != null) {
                    if (selectedTile.currentUnit) {
                        selectedUnit = selectedTile.currentUnit;
                    }
                    ShowSelection(selectedTile);    
                }
                else if (selectedUnit != null) {
                    ShowSelection(selectedUnit);
                }
            }
            else if (mode == SelectionMode.Attacking) {
                if (target)
                    ShowTarget(target);
                else 
                    HideTarget();
            } 
        }
    }

    public static void SaveLastTile(Unit unit) {
        lastTiles[unit] = unit.currentTile;
    }

    public static void ResetLastTile(Unit unit) {
        if(lastTiles[unit]) {
            unit.SetTile(lastTiles[unit]);
        }
    }

    public static void ShowSelection(Tile tile) {
        selectedSpaceObj.transform.position = tile.transform.position + GameResources.visibilityOffset;
    }

    public static void ShowSelection(Unit unit) {
        selectedSpaceObj.transform.position = unit.transform.position + GameResources.visibilityOffset;
    }

    public static void ShowTarget(Unit unit) {
        if (unit)
        {
            target = unit;
            targetSpaceObj.transform.position = unit.currentTile.transform.position + GameResources.visibilityOffset;
        }
    }

    public static void HideSelection() {
        selectedSpaceObj.transform.position = new Vector3(-1000, -1000, 0);
    }

    public static void HideTarget() {
        target = null;
        targetSpaceObj.transform.position = new Vector3(-1000, -1000, 0);
    }

    public static void ClearSelection() {
        selectedTile = null;
        HideSelection();
    }

    public static void ClearAllSelections() {
        selectedTile = null;
        selectedUnit = null;
        target = null;
        HideTarget();
        HideSelection();
    }

    public static bool TakingInput() {
        return mode == SelectionMode.Open;
    }

    public static bool TakingAIInput() {
        return mode == SelectionMode.AITurn;
    }

    public static void SelectFacing() {
        Vector2 directionVec = Input.mousePosition - Camera.main.WorldToScreenPoint(selectedUnit.transform.position);
        selectedUnit.SetFacing(directionVec);
    }
}

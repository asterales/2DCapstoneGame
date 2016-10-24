using UnityEngine;
using System.Collections.Generic;

public class SelectionController : MonoBehaviour {
    public Sprite selectedSprite;
    public Sprite targetSprite;
    public static Unit selectedUnit;
    public static Tile selectedTile;
    public static Unit target;
    public static Dictionary<Unit, Tile> lastTiles = new Dictionary<Unit, Tile>();
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
        sr.sortingOrder = 1;
        selectedSpaceObj.transform.position = GameResources.hidingPosition;
    }

    private void InitTargetSpaceObj() {
        targetSpaceObj = new GameObject(string.Format("Target Space"));
        SpriteRenderer sr = targetSpaceObj.AddComponent<SpriteRenderer>();
        sr.sprite = targetSprite;
        sr.sortingOrder = 1;
        targetSpaceObj.transform.position = GameResources.hidingPosition;
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
        selectedSpaceObj.transform.position = tile.transform.position + GameResources.visibilityOffset + new Vector3(0, 0, 0.1f);
    }

    public static void ShowSelection(Unit unit) {
        selectedSpaceObj.transform.position = unit.transform.position + GameResources.visibilityOffset + new Vector3(0, 0, 0.1f);
    }

    public static void ShowTarget(Unit unit) {
        if (unit)
        {
            target = unit;
            targetSpaceObj.transform.position = unit.currentTile.transform.position + GameResources.visibilityOffset+new Vector3(0, 0, 0.1f);
        }
    }

    public static void HideSelection() {
        selectedSpaceObj.transform.position = GameResources.hidingPosition;
    }

    public static void HideTarget() {
        target = null;
        targetSpaceObj.transform.position = GameResources.hidingPosition;
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

    public static void RegisterFacing() {
        if (SelectionController.selectedUnit) {
            Vector2 directionVec = Input.mousePosition - Camera.main.WorldToScreenPoint(selectedUnit.transform.position);
            selectedUnit.SetFacing(directionVec);
        }
    }
}

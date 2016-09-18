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
    private static readonly Vector3 visibilityOffset = new Vector3(0, 0, -0.01f);

	void Start () {
        mode = SelectionMode.Open;
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


    void Update () {
        if(!SelectionController.TakingAIInput()) {
            if (mode == SelectionMode.Open)
            {
                if (selectedTile != null)
                {
                    if (selectedTile.currentUnit)
                    {
                        selectedUnit = selectedTile.currentUnit;
                    }
                    ShowSelection(selectedTile);
                }
                else if (selectedUnit != null)
                {
                    ShowSelection(selectedUnit);
                }
            }
            else if (mode == SelectionMode.Attacking) {
                ShowTarget(target);
            } else {
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
        selectedSpaceObj.transform.position = tile.transform.position + visibilityOffset;
    }

    public static void ShowSelection(Unit unit) {
        selectedSpaceObj.transform.position = unit.transform.position + visibilityOffset;
    }

    public static void ShowTarget(Unit unit) {
        targetSpaceObj.transform.position = unit.currentTile.transform.position + visibilityOffset;
    }

    public static void HideSelection() {
        selectedSpaceObj.transform.position = new Vector3(-1000, -1000, 0);
    }

    public static void HideTarget() {
        targetSpaceObj.transform.position = new Vector3(-1000, -1000, 0);
    }

    public static void ClearSelection() {
        selectedTile = null;
        HideSelection();
    }

    public static bool TakingInput() {
        return mode == SelectionMode.Open;
    }

    public static bool TakingAIInput() {
        return mode == SelectionMode.AITurn;
    }
}

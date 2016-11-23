using UnityEngine;
using System.Collections.Generic;

public class SelectionController : MonoBehaviour {
    public static SelectionController instance;
    public Sprite selectedSprite;
    public Sprite targetSprite;
    public Unit selectedUnit;
    public Tile selectedTile;
    public Unit target;
    public Dictionary<Unit, Tile> lastTiles;
    public SelectionMode mode;
    private GameObject selectedSpaceObj; // object for selected space
    private GameObject targetSpaceObj; // object for target space

    void Awake() {
        if (instance == null) {
            instance = this;
            Init();
        } else {
            Destroy(this);
        }
    }

    private void Init() {
        mode = SelectionMode.Open;
        lastTiles = new Dictionary<Unit, Tile>();
        InitSelectSpaceObj();
        InitTargetSpaceObj();
        ClearAllSelections();
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
        if(!TakingAIInput()) {
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

    public void SaveLastTile(Unit unit) {
        lastTiles[unit] = unit.currentTile;
    }

    public void ResetLastTile(Unit unit) {
        if(lastTiles[unit]) {
            unit.SetTile(lastTiles[unit]);
        }
    }

    public void ShowSelection(Tile tile) {
        selectedSpaceObj.transform.position = tile.transform.position + GameResources.visibilityOffset + new Vector3(0, 0, 0.1f);
    }

    public void ShowSelection(Unit unit) {
        selectedSpaceObj.transform.position = unit.transform.position + GameResources.visibilityOffset + new Vector3(0, 0, 0.1f);
    }

    public void ShowTarget(Unit unit) {
        if (unit) {
            target = unit;
            if (selectedUnit&&selectedUnit.IsPlayerUnit())
                EnemyUIDrawer.instance.SetPreview(selectedUnit.GetDamage(target));
            if (target.phase == UnitTurn.Open && target.HasInAttackRange(selectedUnit)) {
                target.GetComponent<SpriteRenderer>().color = Color.red;
                if (selectedUnit&& selectedUnit.IsPlayerUnit())
                    PlayerUIDrawer.instance.SetPreview(target.GetDamage(selectedUnit, target.ZOCModifer));
            }
            targetSpaceObj.transform.position = unit.currentTile.transform.position + GameResources.visibilityOffset + new Vector3(0, 0, 0.1f);
        }
    }

    public void HideSelection() {
        selectedSpaceObj.transform.position = GameResources.hidingPosition;
    }

    public void HideTarget() {
        if (target && target.phase == UnitTurn.Open) {
            target.GetComponent<SpriteRenderer>().color = Color.white;
        }
        target = null;
        targetSpaceObj.transform.position = GameResources.hidingPosition;
    }

    public void ClearSelection() {
        selectedTile = null;
        HideSelection();
    }

    public void ClearAllSelections() {
        selectedTile = null;
        selectedUnit = null;
        target = null;
        HideTarget();
        HideSelection();
    }

    public bool TakingInput() {
        return mode == SelectionMode.Open;
    }

    public bool TakingAIInput() {
        return mode == SelectionMode.AITurn;
    }

    public void RegisterFacing() {
        if (selectedUnit) {
            Vector2 directionVec = Input.mousePosition - Camera.main.WorldToScreenPoint(selectedUnit.transform.position);
            selectedUnit.SetFacing(directionVec);
        }
    }
}

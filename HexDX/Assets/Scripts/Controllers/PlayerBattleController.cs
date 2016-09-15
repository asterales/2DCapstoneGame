using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerBattleController : MonoBehaviour {
    public List<Unit> units;
    public static Texture2D menuItem;
    public static Texture2D menuItemHovered;
    private static Tile previousTile;
    public static Unit activeUnit { get; private set; }
    
    // path sprites
    public Sprite circleSprite;
    public Sprite[] lineSprites;
    public Sprite[] arrowSprites;


    void Start() {
        InitUnitList();
    }

    private void InitUnitList() {
        Unit[] allUnits = FindObjectsOfType(typeof(Unit)) as Unit[];
        units = allUnits.Where(unit => unit.isPlayerUnit).ToList();
    }


    void Update(){
        // **See SelectFacing() for similar functionality of confirming facing selection
        //if (BattleController.isPlayerTurn && activeUnit) {
        if (!SelectionController.TakingAIInput() && activeUnit) {
            switch (activeUnit.phase) {
                case UnitTurn.Facing:
                    //SelectionController.SetSelectedTile(activeUnit.currentTile);
                    SelectionController.selectedTile = activeUnit.currentTile;
                    SelectFacing();
                    break;
                case UnitTurn.Done:
                    ClearActiveUnit();
                    //SelectionController.OpenTileSelection();
                    SelectionController.selectionMode = SelectionMode.Open;
                    break;
            }
        }
    }

    public static void SetActiveUnit(Unit unit) {
        if (unit.isPlayerUnit) {
            activeUnit = unit;
            previousTile = unit.currentTile;
        } else {
            Debug.Log("PlayerBattleController.cs - attempted to set enemy unit as active unit");
        }
    }

    public static void ClearActiveUnit() {
        activeUnit = null;
    }

    private void SelectFacing() {
        Vector2 directionVec = Input.mousePosition - CameraController.camera.WorldToScreenPoint(activeUnit.transform.position);
        activeUnit.SetFacing(directionVec);
        if (Input.GetMouseButtonDown(1) && SelectionController.selectedUnit == activeUnit) {
            activeUnit.MakeChoosingAction();
        }
    }

    void OnGUI() {
        if (InUnitPhase(UnitTurn.ChoosingAction)) {
            int itemHeight = 20;
            int itemWidth = 60;
            int offset = 60;
            Vector3 pos = CameraController.camera.WorldToScreenPoint(activeUnit.transform.position);
            pos = new Vector3(pos.x, Screen.height - pos.y-offset);
            bool canAttack = HexMap.GetAttackTiles(activeUnit.currentTile).FirstOrDefault(t => t.currentUnit && !t.currentUnit.isPlayerUnit);
            if (GUI.Button(new Rect(pos.x, pos.y, itemWidth, itemHeight), " Attack", GetGUIStyle(canAttack))) {
                activeUnit.MakeAttacking();
                //SelectionController.RestrictToAttackTiles();
            }
            if (GUI.Button(new Rect(pos.x, pos.y+ itemHeight, itemWidth, itemHeight), " Wait", GetGUIStyle(true))) {
                activeUnit.MakeDone();
            }
            if (GUI.Button(new Rect(pos.x, pos.y + 2*itemHeight, itemWidth, itemHeight), " Undo", GetGUIStyle(true))) {
                activeUnit.SetTile(previousTile);
                activeUnit.MakeOpen();
                HexMap.ClearAllTiles();
                HexMap.ShowMovementTiles(activeUnit.currentTile, activeUnit.unitStats.mvtRange + 1);
                MovementTile.path = new List<Tile>() { activeUnit.currentTile };
                //SelectionController.SetSelectedTile(previousTile);
                SelectionController.selectedTile = previousTile;
                //SelectionController.OpenTileSelection();
                SelectionController.selectionMode = SelectionMode.Open;
            }
        }
    }

    public GUIStyle GetGUIStyle(bool active) {
        GUIStyle style = new GUIStyle();
        style.normal.background = menuItem;
        style.alignment = TextAnchor.MiddleLeft;
        if (active) {
            style.hover.background = menuItemHovered;
            style.normal.textColor = Color.white;
            style.hover.textColor = Color.white;
        } else {
            style.hover.background = menuItem;
            style.normal.textColor = Color.gray;
            style.hover.textColor = Color.gray;
        }
        return style;
    }

    public void StartTurn() {
        SelectionController.selectionMode = SelectionMode.Open;
        //SelectionController.OpenTileSelection();
    }

    public void EndTurn() {
        ClearSelections();
        for (int i = 0; i < units.Count; i++) {
            if (units[i]){
                units[i].MakeOpen();    
            } 
        }
    }

    private void ClearSelections() {
        if (activeUnit) {
            activeUnit.MakeDone();
            ClearActiveUnit();
        }
        HexMap.ClearAllTiles();
        SelectionController.ClearSelection();
        MovementTile.path = null;
    }

    public static bool InUnitPhase(UnitTurn phase) {
        //return BattleController.isPlayerTurn && activeUnit && activeUnit.phase == phase;
        return !SelectionController.TakingAIInput() && activeUnit && activeUnit.phase == phase;
    }
}

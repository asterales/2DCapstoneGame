using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerBattleController : ArmyBattleController {
    public static Texture2D menuItem;
    public static Texture2D menuItemHovered;
    
    // for ease of use
    private static Unit selectedUnit { 
        get { return SelectionController.selectedUnit; } 
        set { SelectionController.selectedUnit = value; }
    }
    
    // path sprites
    public Sprite circleSprite;
    public Sprite[] lineSprites;
    public Sprite[] arrowSprites;

    public override void InitUnitList() {
        Unit[] allUnits = FindObjectsOfType(typeof(Unit)) as Unit[];
        units = allUnits.Where(unit => unit.IsPlayerUnit()).ToList();
    }

    void Update(){
        if (!SelectionController.TakingAIInput() && selectedUnit) {
            switch (selectedUnit.phase) {
                case UnitTurn.Facing:
                    SelectionController.selectedTile = selectedUnit.currentTile;
                    SelectFacing();
                    break;
                case UnitTurn.Done:
                    selectedUnit = null;
                    SelectionController.mode = SelectionMode.Open;
                    break;
            }
        }
        if (!battleController.BattleIsDone && battleController.CanEndTurn()) {
            battleController.EndCurrentTurn();
        }
    }

    private void SelectFacing() {
        SelectionController.SelectFacing();
        if (Input.GetMouseButtonDown(1)) {
            selectedUnit.MakeChoosingAction();
        }
    }

    void OnGUI() {
        if (InUnitPhase(UnitTurn.ChoosingAction)) {
            int itemHeight = 20;
            int itemWidth = 60;
            int offset = 60;
            Vector3 pos = Camera.main.WorldToScreenPoint(selectedUnit.transform.position);
            pos = new Vector3(pos.x, Screen.height - pos.y-offset);
            Tile tile = HexMap.GetAttackTiles(selectedUnit).FirstOrDefault(t => t.currentUnit && !t.currentUnit.IsPlayerUnit());
            bool canAttack = tile;

            if (GUI.Button(new Rect(pos.x, pos.y, itemWidth, itemHeight), " Attack", GetGUIStyle(canAttack))) {
                SelectionController.target = tile.currentUnit;
                selectedUnit.MakeAttacking();
            }
            if (GUI.Button(new Rect(pos.x, pos.y+ itemHeight, itemWidth, itemHeight), " Wait", GetGUIStyle(true))) {
                selectedUnit.MakeDone();
            }
            if (GUI.Button(new Rect(pos.x, pos.y + 2*itemHeight, itemWidth, itemHeight), " Undo", GetGUIStyle(true))) {
                SelectionController.ResetLastTile(selectedUnit);
                SelectionController.selectedTile = selectedUnit.currentTile;
                SelectionController.mode = SelectionMode.Open;
                selectedUnit.MakeOpen();
                HexMap.ClearAllTiles();
                HexMap.ShowMovementTiles(selectedUnit);
                MovementTile.path = new List<Tile>() { selectedUnit.currentTile };
            }
        }
    }

    public GUIStyle GetGUIStyle(bool active) {
        GUIStyle style = new GUIStyle();
        style.normal.background = menuItem;
        style.alignment = TextAnchor.MiddleLeft;
        if (active) {
            GUI.enabled = true;
            GUI.color = Color.white;
            style.hover.background = menuItemHovered;
            style.normal.textColor = Color.white;
            style.hover.textColor = Color.white;
        } else {
            GUI.enabled = false;
            GUI.color = new Color(1, 1, 1, 2) ;
            style.hover.background = menuItem;
            style.normal.textColor = Color.gray;
            style.hover.textColor = Color.gray;
        }
        return style;
    }

    public override void StartTurn() {
        SelectionController.mode = SelectionMode.Open;
        base.StartTurn();
    }

    public override void EndTurn() {
        ClearSelections();
        base.EndTurn();
    }

    private void ClearSelections() {
        if (selectedUnit) {
            selectedUnit.MakeDone();
            selectedUnit = null;
        }
        HexMap.ClearAllTiles();
        SelectionController.ClearSelection();
        MovementTile.path = null;
    }

    public static bool InUnitPhase(UnitTurn phase) {
        return !SelectionController.TakingAIInput() && selectedUnit && selectedUnit.phase == phase;
    }
}

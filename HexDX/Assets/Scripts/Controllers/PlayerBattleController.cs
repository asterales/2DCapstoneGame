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
        if (!SelectionController.TakingAIInput() 
            && SelectionController.mode != SelectionMode.TurnTransition 
            && selectedUnit == null) {
            SelectionController.mode = SelectionMode.Open;
        }
        if (!SelectionController.TakingAIInput() 
            && SelectionController.mode != SelectionMode.TurnTransition 
            && selectedUnit) {
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
        SelectionController.RegisterFacing();
        if (Input.GetMouseButtonDown(1)) {
            selectedUnit.MakeChoosingAction();
        }
    }

    void OnGUI() {
        if (InUnitPhase(UnitTurn.ChoosingAction)) {
            Vector3 pos = Camera.main.WorldToScreenPoint(selectedUnit.transform.position);
            Tile tile = HexMap.GetAttackTiles(selectedUnit).FirstOrDefault(t => t.currentUnit && !t.currentUnit.IsPlayerUnit());
            bool canAttack = tile;

            if (GetSubmenuButton(pos, 1, "Attack", canAttack)) {
                SelectionController.target = tile.currentUnit;
                selectedUnit.MakeAttacking();
            }
            if (GetSubmenuButton(pos, 2, "Wait", true)) {
                selectedUnit.MakeDone();
            }
            if (GetSubmenuButton(pos, 3, "Undo", true)) {
                UnitState.RestoreStates();
                SelectionController.selectedTile = selectedUnit.currentTile;
                SelectionController.mode = SelectionMode.Open;
                selectedUnit.MakeOpen();
                HexMap.ClearAllTiles();
                HexMap.ShowMovementTiles(selectedUnit);
                MovementTile.path = new List<Tile>() { selectedUnit.currentTile };
            }
        }
    }

    public bool GetSubmenuButton(Vector3 basePosition, int menuButtonNumber, string text, bool active) {
        float itemHeight = Screen.width * (0.15f / Camera.main.orthographicSize);
        float itemWidth = itemHeight * 3;
        float yOffset = itemHeight * 4f;
        float xOffset = itemWidth / 4;
        Vector3 pos = new Vector3(basePosition.x + xOffset, Screen.height - basePosition.y - yOffset);
        menuButtonNumber = menuButtonNumber >= 1 ? menuButtonNumber : 1;
        float menuButtonOffset = itemHeight * (menuButtonNumber - 1);
        string buttonText = " " + text;
        return GUI.Button(new Rect(pos.x, pos.y + menuButtonOffset, itemWidth, itemHeight), buttonText, GetGUIStyle(active, itemHeight));
    }

    private GUIStyle GetGUIStyle(bool active, float itemHeight) {
        GUIStyle style = new GUIStyle();
        style.normal.background = menuItem;
        style.alignment = TextAnchor.MiddleLeft;
        style.fontSize = (int) (itemHeight * 0.6);
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
        Debug.Log("Starting Player Turn");
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

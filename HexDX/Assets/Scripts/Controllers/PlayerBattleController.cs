using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerBattleController : ArmyBattleController {
    public Texture2D menuItem;
    public Texture2D menuItemHovered;
    
    // path sprites
    public Sprite circleSprite;
    public Sprite[] lineSprites;
    public Sprite[] arrowSprites;

    private Dictionary<Unit, UnitState> initialStates;

    // for ease of use
    private Unit selectedUnit { 
        get { return sc.selectedUnit; } 
        set { sc.selectedUnit = value; }
    }

    public override void InitUnitList() {
        Unit[] allUnits = BattleControllerManager.instance.hexMap.GetUnitsOnMap();
        units = allUnits.Where(unit => unit.IsPlayerUnit()).ToList();
        initialStates = new Dictionary<Unit, UnitState>();
        foreach(Unit unit in units) {
            initialStates[unit] = new UnitState(unit.phase, null, unit.facing, unit.Health, unit.Experience, unit.Veterancy);
        }
    }

    void Update() {
        if (!sc.TakingAIInput()
            && sc.mode != SelectionMode.TurnTransition
            && selectedUnit == null) {
            sc.mode = SelectionMode.Open;
        }
        if (!sc.TakingAIInput()
            && sc.mode != SelectionMode.TurnTransition
            && selectedUnit) {
            if (selectedUnit.phase == UnitTurn.Moving || selectedUnit.phase == UnitTurn.Facing || selectedUnit.phase == UnitTurn.ChoosingAction)
                if (Input.GetKeyDown(KeyCode.LeftShift)||Input.GetMouseButtonDown(2))
                {
                    Undo();
                }
            switch (selectedUnit.phase) {
                case UnitTurn.Attacking:
                    break;
                case UnitTurn.Facing:
                    sc.selectedTile = selectedUnit.currentTile;
                    SelectFacing();
                    break;
                case UnitTurn.Done:
                    selectedUnit = null;
                    sc.mode = SelectionMode.Open;
                    break;
            }
        }
        if (!battleController.BattleIsDone && battleController.CanEndTurn()) {
            battleController.EndCurrentTurn();
        }
    }

    private void SelectFacing() {
        sc.RegisterFacing();
        if (Input.GetMouseButtonDown(1)) {
            sc.RegisterFacing();
            selectedUnit.MakeChoosingAction();
        }
    }

    void OnGUI() {
        if (InUnitPhase(UnitTurn.ChoosingAction)) {
            Vector3 pos = Camera.main.WorldToScreenPoint(selectedUnit.transform.position);
            Tile tile = HexMap.GetAttackTiles(selectedUnit).FirstOrDefault(t => t.currentUnit && !t.currentUnit.IsPlayerUnit());
            bool canAttack = tile;

            if (GetSubmenuButton(pos, 1, "Attack", canAttack)) {
                sc.target = tile.currentUnit;
                selectedUnit.MakeAttacking();
            }
            if (GetSubmenuButton(pos, 2, "Wait", true)) {
                selectedUnit.MakeDone();
            }
            if (GetSubmenuButton(pos, 3, "Undo", true)) {
                Undo();
            }
        }
    }

    public void Undo()
    {
        UnitState.RestoreStates();
        sc.selectedTile = selectedUnit.currentTile;
        sc.mode = SelectionMode.Open;
        selectedUnit.MakeOpen();
        HexMap.ClearAllTiles();
        HexMap.ShowMovementTiles(selectedUnit);
        MovementTile.path = new List<Tile>() { selectedUnit.currentTile };
    }
    public bool GetSubmenuButton(Vector3 basePosition, int menuButtonNumber, string text, bool active) {
        float itemHeight = Screen.height*0.026f;
        float itemWidth = itemHeight*3;
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
        sc.mode = SelectionMode.Open;
        base.StartTurn();
    }

    public override void EndTurn() {
        ClearSelections();
        base.EndTurn();
    }

    public void RestoreInitialArmyState() {
        foreach(Unit unit in units) {
            initialStates[unit].ChangeStateOf(unit);
        }
    }

    private void ClearSelections() {
        if (selectedUnit) {
            selectedUnit.MakeDone();
            selectedUnit = null;
        }
        HexMap.ClearAllTiles();
        sc.ClearSelection();
        MovementTile.path = null;
    }

    public bool InUnitPhase(UnitTurn phase) {
        return !sc.TakingAIInput() && selectedUnit && selectedUnit.phase == phase;
    }
}

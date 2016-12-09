using UnityEngine;
using System.Collections.Generic;
using MovementEffects;

public class MovementTile : MonoBehaviour {

    public Tile tile;
    private SelectionController sc;
    public static List<Tile> path;
    private static Color pathColor = new Color(1.0f, 1.0f, 1.0f, 0.6f);

    void Start() {
        sc = SelectionController.instance;
    }
   
    public void OnMouseOver() {
        if (sc.TakingInput()) {
            if (Input.GetMouseButtonDown(0)) {
                tile.OnMouseOver();
            } else if (Input.GetMouseButtonDown(1)) {
                if (path != null && (tile.currentUnit == sc.selectedUnit || tile.currentUnit == null)) {
                    MusicController.instance.PlaySelectSfx();
                    StartCoroutine(CommitPath());
                } else {
                    tile.OnMouseOver();
                }
            }
        }
        TutorialController tutorial = BattleManager.instance.tutorial;
        if (tutorial && tutorial.enabled && tutorial.IsTargetDestination(this)){
            if (path != null && path[path.Count - 1] != tile && !HexMap.AreNeighbors(tile, path[path.Count - 1]) ){
                path = sc.selectedUnit.GetShortestPath(tile);
                DrawPath();
            }
            if (Input.GetMouseButtonDown(1)){
                MusicController.instance.PlaySelectSfx();
                StartCoroutine(CommitPath(tutorial.eventsList.currentScriptEvent as ScriptedMove));
            }
        }

    }
    
    public void OnMouseEnter() {
        sc.HideTarget();
        PlayerUIDrawer.instance.SetPreview(0);
        EnemyUIDrawer.instance.SetPreview(0);
        if ((sc.TakingInput() 
                || sc.mode == SelectionMode.ScriptedPlayerMove) 
                && path != null) {
            if (path.Count > 1 && tile == path[path.Count - 2]) {
                // Backtracking - remove last tile
                path.RemoveAt(path.Count - 1);
            } else if (HexMap.AreNeighbors(tile, path[path.Count - 1])
                        && sc.selectedUnit.CanPathThrough(tile)) {
                if (PathCost(path) + (int)tile.tileStats.mvtDifficulty <= sc.selectedUnit.MvtRange) {
                    path.Add(tile);
                } else {
                   path = sc.selectedUnit.GetShortestPath(tile);
                }
            }
            else {
                path = sc.selectedUnit.GetShortestPath(tile);
            }
            DrawPath();
        }
    }


    public static int PathCost(List<Tile> path) {
        int cost = 0;
        for (int i = 1; i < path.Count; i++)
            cost+=(int)path[i].tileStats.mvtDifficulty;
        return cost;
    }

    public static IEnumerator<WaitForEndOfFrame> CommitPath(ScriptedMove move = null) {
        SelectionController sc = SelectionController.instance;
       if (path != null && (path[path.Count - 1].currentUnit == null || path[path.Count - 1].currentUnit == sc.selectedUnit)) {
            if (path.Count > 1)
            {
                sc.selectedUnit.MakeMoving(move);
                sc.selectedUnit.SetPath(path);
                if (move == null)
                {
                    sc.mode = SelectionMode.Moving;
                }
            }
            else
            {
                yield return new WaitForEndOfFrame();
                sc.mode = SelectionMode.Facing;
                sc.selectedUnit.MakeFacing();
            }
            sc.SaveLastTile(sc.selectedUnit);
            path = null;
            HexMap.ClearAllTiles();
            sc.ClearSelection();
        }
    }

    public static void DrawPath() {
        PlayerBattleController pbc = BattleManager.instance.player;
        Object.Destroy(GameObject.Find("path"));
        if (path!=null) {
            if (path.Count > 1)
            {
                GameObject pathDraw = new GameObject("path");
                Tile prev = path[0];
                for (int i = 1; i < path.Count; i++)
                {
                    DrawCircle(pathDraw, prev, pbc.circleSprite);
                    DrawLine(pathDraw, prev, path[i], pbc.lineSprites);
                    prev = path[i];
                }
                DrawArrow(pathDraw, path[path.Count - 2], prev, pbc.arrowSprites);
            }
            DrawZoneOfControlThreat();
        }
    }

    private static void DrawZoneOfControlThreat() {
        SelectionController sc = SelectionController.instance;
        int preview = 0;
        foreach (Unit unit in BattleController.instance.ai.units){
            if (unit.phase == UnitTurn.Open){
                bool inrange = false;
                foreach (Tile tile in path)
                {
                    if (HexMap.GetAttackTiles(unit).Contains(tile))
                    {
                        inrange = true;
                        unit.spriteRenderer.color = Color.red;
                        break;
                    }
                }
                if (!inrange || path.Count==1)
                    unit.spriteRenderer.color = Color.white;
                if (unit == sc.target && unit.HasInAttackRange(sc.selectedUnit))
                    unit.spriteRenderer.color = Color.red;
                if (unit.spriteRenderer.color == Color.red)
                    preview += unit.GetDamage(sc.selectedUnit, unit.ZOCModifer);
            }
        }

        PlayerUIDrawer.instance.SetPreview(preview);
    }

    private static void DrawCircle(GameObject pathObj, Tile tile, Sprite circleSprite) {
        GameObject circleObj = new GameObject();
        circleObj.transform.parent = pathObj.transform;
        SpriteRenderer circle = circleObj.AddComponent<SpriteRenderer>();
        circle.color = pathColor;
        circle.sortingOrder = 2;
        circle.sprite = circleSprite;
        circle.transform.position = tile.transform.position;
    }

    private static void DrawLine(GameObject pathObj, Tile startTile, Tile endTile, Sprite[] lineSprites) {
        GameObject lineObj = new GameObject();
        lineObj.transform.parent = pathObj.transform;
        SpriteRenderer line = lineObj.AddComponent<SpriteRenderer>();
        line.color = pathColor;
        line.sortingOrder = 2;
        line.sprite = lineSprites[GetDirection(startTile, endTile)];
        line.transform.position = startTile.transform.position;
    }

    private static void DrawArrow(GameObject pathObj, Tile startTile, Tile endTile, Sprite[] arrowSprites){
        GameObject arrowObj = new GameObject();
        arrowObj.transform.parent = pathObj.transform;
        SpriteRenderer arrow = arrowObj.AddComponent<SpriteRenderer>();
        arrow.color = pathColor;
        arrow.sortingOrder = 2;
        arrow.sprite = arrowSprites[GetDirection(startTile, endTile)];
        arrow.transform.position = endTile.transform.position;
    }

    private static int GetDirection(Tile startTile, Tile endTile) {
        return HexMap.GetNeighbors(startTile).IndexOf(endTile);
    }
}

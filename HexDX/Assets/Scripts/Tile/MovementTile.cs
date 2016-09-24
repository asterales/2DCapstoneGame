using UnityEngine;
using System.Collections.Generic;

public class MovementTile : MonoBehaviour {

    public Tile tile;
    public static List<Tile> path;
    private static Color pathColor = new Color(1.0f, 1.0f, 1.0f, 0.6f);
   
    public void OnMouseOver() {
        if (SelectionController.TakingInput()) {
            if (Input.GetMouseButtonDown(0)) {
                tile.OnMouseOver();
            } else if (Input.GetMouseButtonDown(1)) {
                CommitPath();
            }
        } 
    }
    
    public void OnMouseEnter() {
        if (SelectionController.TakingInput() && path != null) {
            if (path.Count > 1 && tile == path[path.Count - 2]) {
                // Backtracking - remove last tile
                path.RemoveAt(path.Count - 1);
            } else if (HexMap.AreNeighbors(tile, path[path.Count - 1])
                        && SelectionController.selectedUnit.CanPathThrough(tile)) {
                if (path.Count <= SelectionController.selectedUnit.MvtRange) {
                    path.Add(tile);
                } else {
                   path = SelectionController.selectedUnit.GetShortestPath(tile);
                }
            }
            else {
                path = SelectionController.selectedUnit.GetShortestPath(tile);
            }
            DrawPath();
        }
    }

    public static void CommitPath() {   
        if (path != null && (path[path.Count - 1].currentUnit == null || path[path.Count - 1].currentUnit == SelectionController.selectedUnit)) {
            SelectionController.selectedUnit.MakeMoving(null);
            SelectionController.selectedUnit.SetPath(path);
            SelectionController.mode = SelectionMode.Moving;
            SelectionController.SaveLastTile(SelectionController.selectedUnit);
            path = null;
            HexMap.ClearMovementTiles();
            SelectionController.ClearSelection();
        }
    }

    private void DrawPath() {
        PlayerBattleController pbc = GameObject.Find("TestHexMap").GetComponent<PlayerBattleController>();
        Object.Destroy(GameObject.Find("path"));
        if (path!=null && path.Count > 1) {
            GameObject pathDraw = new GameObject("path");
            Tile prev = path[0];
            for (int i = 1; i < path.Count; i++) {
                DrawCircle(pathDraw, prev, pbc.circleSprite);
                DrawLine(pathDraw, prev, path[i], pbc.lineSprites);
                prev = path[i];
            }
            DrawArrow(pathDraw, path[path.Count - 2], prev, pbc.arrowSprites);
        }
    }

    private void DrawCircle(GameObject pathObj, Tile tile, Sprite circleSprite) {
        GameObject circleObj = new GameObject();
        circleObj.transform.parent = pathObj.transform;
        SpriteRenderer circle = circleObj.AddComponent<SpriteRenderer>();
        circle.color = pathColor;
        circle.sortingOrder = 1;
        circle.sprite = circleSprite;
        circle.transform.position = tile.transform.position;
    }

    private void DrawLine(GameObject pathObj, Tile startTile, Tile endTile, Sprite[] lineSprites) {
        GameObject lineObj = new GameObject();
        lineObj.transform.parent = pathObj.transform;
        SpriteRenderer line = lineObj.AddComponent<SpriteRenderer>();
        line.color = pathColor;
        line.sortingOrder = 1;
        line.sprite = lineSprites[GetDirection(startTile, endTile)];
        line.transform.position = startTile.transform.position;
    }

    private void DrawArrow(GameObject pathObj, Tile startTile, Tile endTile, Sprite[] arrowSprites){
        GameObject arrowObj = new GameObject();
        arrowObj.transform.parent = pathObj.transform;
        SpriteRenderer arrow = arrowObj.AddComponent<SpriteRenderer>();
        arrow.color = pathColor;
        arrow.sortingOrder = 1;
        arrow.sprite = arrowSprites[GetDirection(startTile, endTile)];
        arrow.transform.position = endTile.transform.position;
    }

    private int GetDirection(Tile startTile, Tile endTile) {
        return HexMap.GetNeighbors(startTile).IndexOf(endTile);
    }
}

using UnityEngine;
using System.Collections.Generic;

public class MovementTile : MonoBehaviour {

    public Tile tile;
    public static List<Tile> path;
   
    public void OnMouseDown() {
        if (SelectionController.TakingInput()) {
            CommitPath();
        }
    }
    
    public void OnMouseEnter() {
        if (SelectionController.TakingInput()) {
            if (path != null) {
                if (path.Count > 1 && tile == path[path.Count - 2]) {
                    path.RemoveAt(path.Count - 1);
                } else {
                    if (path.Count <= SelectionController.selectedUnit.unitStats.mvtRange
                            && HexMap.AreNeighbors(tile, path[path.Count - 1])) {
                        path.Add(tile);
                    } else {
                        path = HexMap.GetShortestPath(tile, SelectionController.selectedTile);
                    }
                }
                drawPath();
            }
        }
    }

    public static void CommitPath() {
        if (path != null && path.Count > 1 && path[path.Count - 1].currentUnit == null) {
            SelectionController.selectedUnit.SetPath(path);
            path = null;
            HexMap.ClearMovementTiles();
            SelectionController.ClearSelection();
        }
    }

    //refactor later
    public void drawPath() {
        PlayerBattleController pbc = GameObject.Find("TestHexMap").GetComponent<PlayerBattleController>();
        Object.Destroy(GameObject.Find("path"));
        if (path!=null && path.Count > 1) {
            GameObject pathDraw = new GameObject("path");
            Tile prev = path[0];
            int direction= 0;
            GameObject temp;
            for (int i = 1; i < path.Count; i++) {
                temp = new GameObject();
                temp.transform.parent = pathDraw.transform;
                SpriteRenderer circle = temp.AddComponent<SpriteRenderer>();
                circle.sortingOrder = 1;
                circle.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
                circle.sprite = pbc.circleSprite;
                circle.transform.position = prev.transform.position;
                direction = HexMap.GetNeighbors(prev).IndexOf(path[i]);
                temp = new GameObject();
                temp.transform.parent = pathDraw.transform;
                SpriteRenderer line = temp.AddComponent<SpriteRenderer>();
                line.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
                line.sortingOrder = 1;
                Debug.Log(direction);
                line.sprite = pbc.lineSprites[direction];
                line.transform.position = prev.transform.position;
                prev = path[i];
            }
            direction = HexMap.GetNeighbors(path[path.Count - 2]).IndexOf(prev);
            temp = new GameObject();
            temp.transform.parent = pathDraw.transform;
            SpriteRenderer arrow = temp.AddComponent<SpriteRenderer>();
            arrow.sprite = pbc.arrowSprites[direction];
            arrow.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);
            arrow.sortingOrder = 1;
            arrow.transform.position = prev.transform.position;
        }
    }
}

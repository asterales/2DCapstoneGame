using UnityEngine;
using System.Collections.Generic;

public class PlayerBattleController : MonoBehaviour {
    public List<Unit> units;
	private SelectionController selectionController;
	private Tile unitTile;
	private Tile destinationTile;
    
    // path sprites
    public Sprite circleSprite;
    public Sprite[] lineSprites;
    public Sprite[] arrowSprites;

	void Awake() {
        units = new List<Unit>();
		selectionController = GameObject.Find("TestHexMap").GetComponent<HexMap>().GetComponent<SelectionController>();
	}

	void Update(){
        if (SelectionController.selectedUnit!=null && SelectionController.selectedUnit.phase == UnitTurn.Facing && Input.GetMouseButtonDown(0))
        {
            SelectionController.selectedUnit.MakeDone();
        }
    }

    public void StartTurn()
    {
        //for (int i=0;i<units.Count;i++)
        //{
        //    units[i].phase = UnitTurn.Open;
        //}
    }

    public void EndTurn()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].MakeOpen();
        }
    }

    public void AddUnit(Unit unit)
    {
        units.Add(unit);
    }
}

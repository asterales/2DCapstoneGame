using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TutorialMapLoader : MonoBehaviour
{
    public List<Sprite> sprites;
    public string csvMapFile = "Assets/Maps/test.csv";
    public int numCols;
    public int numRows;

    private HexMap battleMap;
    private HexDimension hexDimension;
    private BattleController battleController;
    private BattleSpriteCache spriteCache;

    void Start()
    {
        battleMap = this.gameObject.GetComponent<HexMap>();
        hexDimension = this.gameObject.GetComponent<HexDimension>();
        battleController = this.gameObject.GetComponent<BattleController>();
        spriteCache = this.gameObject.GetComponent<BattleSpriteCache>();

        ////// DEBUG CODE //////
        if (battleMap == null)
        {
            Debug.Log("BattleMap needs to be set -> TutorialMapLoader.cs");
        }
        if (hexDimension == null)
        {
            Debug.Log("HexDimension needs to be set -> TutorialMapLoader.cs");
        }
        if (battleMap.selectionController == null)
        {
            Debug.Log("Major Error :: Hex Map Needs Selection Controller -> TutorialMapLoader.cs");
        }
        if (numRows <= 0 || numCols <= 0)
        {
            Debug.Log("Major Error :: Cols and Rows need to be more than one -> TutorialMapLoader.cs");
        }
        if (HexMap.mapArray.Count != numRows)
        {
            Debug.Log("Major Error :: Hex Map Rows and Loader Rows are not the same -> TutorialMapLoader.cs");
        }
        if (HexMap.mapArray.Count != numCols)
        {
            Debug.Log("Major Error :: Hex Map Cols and Loader Cols are not the same -> TutorialMapLoader.cs");
        }
        ////////////////////////

        if (battleMap != null && hexDimension != null)
        {
            // load the test battle map
            LoadHexMap(csvMapFile);
        }
    }

    private void LoadHexMap(string hexMapFile)
    {
        var reader = new StreamReader(File.OpenRead(hexMapFile));
        battleMap.ClearMap();

        RowContainer rowContainer = battleMap.rowContainer;
        RowOfTiles row;

        float x = 0;
        float y = 0;
        float z = 0;
        int rowIndex = 0;

        while (!reader.EndOfStream)
        {
            int columnIndex = 0;
            string[] line = reader.ReadLine().Split(',');
            row = rowContainer.tileRows[rowIndex];

            foreach (string num in line)
            {
                GameObject tileObj = row.tiles[columnIndex].gameObject;
                SetTile(tileObj, int.Parse(num), new Vector3(x, y, z), rowIndex, columnIndex, row);
                x += 2 * hexDimension.width;
                columnIndex++;
            }
            y -= 2 * hexDimension.apex - hexDimension.minorApex;
            x -= 2 * hexDimension.width * line.Length - hexDimension.width;
            z -= .001f;
            rowIndex++;
        }

        HexMap.LoadMapFromRowContainer(rowContainer);
    }

    private void SetTile(GameObject tileObj, int val, Vector3 pos, int row, int col, RowOfTiles rowObj)
    {
        int type = val / 100;
        int variant = val % 100;
        SpriteRenderer tileRenderer = tileObj.GetComponent<SpriteRenderer>();
        tileRenderer.sprite = spriteCache.GetTileSprite(type, variant);
        if (tileObj != null)
        {
            tileObj.name = string.Format("Tile ({0}, {1})", row, col);
            tileObj.transform.localPosition = pos;
            rowObj.tiles[col].selectionController = battleMap.selectionController;
            TileLocation location = rowObj.tiles[col].position;
            location.col = col;
            location.row = row;
        }
    }

    // keeping for reference
    /*private void AddUnitToTile(int row, int col, bool isPlayerUnit, Vector3 facing)
    {
        GameObject unitObject = null;
        if (isPlayerUnit)
            unitObject = Instantiate(Resources.Load("Units/Rifleman")) as GameObject;
        else
            unitObject = Instantiate(Resources.Load("Units/Swordsman")) as GameObject;
        Unit unit = unitObject.GetComponent<Unit>();
        unit.SetTile(HexMap.mapArray[row][col]);
        unit.SetFacing(facing);
        if (!isPlayerUnit)
        {
            unitObject.AddComponent<BasicUnitAI>();
        }
    }*/
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialMapLoader : MonoBehaviour
{
    private static readonly string mapsDir = "Maps/";
    public string csvMapFile = "test";
    public int numCols;
    public int numRows;

    private HexMap battleMap;
    private HexDimension hexDimension;
    private ScriptedAIBattleController scriptedAI;
    private BattleSpriteCache spriteCache;

    void Start()
    {
        battleMap = this.gameObject.GetComponent<HexMap>();
        hexDimension = this.gameObject.GetComponent<HexDimension>();
        scriptedAI = this.gameObject.GetComponent<ScriptedAIBattleController>();
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
        ////////////////////////

        if (battleMap != null && hexDimension != null)
        {
            // load the test battle map
            LoadHexMap(csvMapFile);
            AddUnitToTile(5, 5, scriptedAI.aiUnits[0], false, new Vector3(0, 1, 0));
            AddUnitToTile(10, 4, scriptedAI.aiUnits[1], false, new Vector3(0, 1, 0));
            AddUnitToTile(6, 6, scriptedAI.aiUnits[2], false, new Vector3(0, 1, 0));
            AddUnitToTile(4, 2, scriptedAI.aiUnits[3], true, new Vector3(0, 1, 0));
            AddUnitToTile(3, 3, scriptedAI.aiUnits[4], true, new Vector3(0, 1, 0));
        }

        ////// DEBUG CODE //////
        if (HexMap.mapArray.Count != numRows)
        {
            Debug.Log("Major Error :: Hex Map Rows and Loader Rows are not the same -> TutorialMapLoader.cs");
        }
        if (HexMap.mapArray.Count != numCols)
        {
            Debug.Log("Major Error :: Hex Map Cols and Loader Cols are not the same -> TutorialMapLoader.cs");
        }
        ////////////////////////
    }

    private void LoadHexMap(string hexMapFile)
    {
        TextAsset hexMapLines = Resources.Load<TextAsset>(mapsDir + hexMapFile);
        string[] mapCsvRows = hexMapLines.text.Trim().Split('\n');
        battleMap.ClearMap();

        RowContainer rowContainer = battleMap.rowContainer;
        RowOfTiles row;

        float x = 0;
        float y = 0;
        float z = 0;
        int rowIndex = 0;

        for (int i=1;i<mapCsvRows.Length-1; i++) {
            string csvRow = mapCsvRows[i];
            int columnIndex = 0;
            string[] line = csvRow.Trim().Split(',');
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
        if (type > 1) tileObj.GetComponent<Tile>().pathable = false;
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

    // keeping for debugging
    private void AddUnitToTile(int row, int col, Unit unit, bool isPlayerUnit, Vector3 facing)
    {
        unit.SetTile(HexMap.mapArray[row][col]);
        unit.SetFacing(facing);
        if (!isPlayerUnit)
        {
            unit.gameObject.AddComponent<BasicUnitAI>();
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MapLoader : MonoBehaviour {
    public List<Sprite> sprites;
    public string csvMapFile = "Assets/Maps/oldTest.csv";

    private HexMap battleMap;
    private HexDimension hexDimension;
    private BattleController battleController;
    private BattleSpriteCache spriteCache;

    void Awake() {
        battleMap = this.gameObject.GetComponent<HexMap>();
        hexDimension = this.gameObject.GetComponent<HexDimension>();
        battleController = this.gameObject.GetComponent<BattleController>();
        spriteCache = this.gameObject.GetComponent<BattleSpriteCache>();

        ////// DEBUG CODE //////
        if (battleMap == null)
        {
            Debug.Log("BattleMap needs to be set -> MapLoader.cs");
        }
        if (hexDimension == null)
        {
            Debug.Log("HexDimension needs to be set -> MapLoader.cs");
        }
        if (battleMap.selectionController == null)
        {
            Debug.Log("Major Error :: Hex Map Needs Selection Controller");
        }
        ////////////////////////
    }

    // row and colum indices start at 0 at upper left corner
    void Start() {
        if (battleMap != null && hexDimension != null) {
            // load the test battle map
            LoadHexMap(csvMapFile);
            LoadUnits();
        }
    }

    void LoadHexMap(string hexMapFile) {
        var reader = new StreamReader(File.OpenRead(hexMapFile));
        battleMap.ClearMap();

        List<Tile> row;
        float x = 0;
        float y = 0;
        float z = 0;
        int rowIndex = 0;

        while (!reader.EndOfStream) {
            // Create new object for row in map, make it a subobject of hexMap
            // row objects are useless other than for organizational purposes so putting DEBUG around them
            ////// DEBUG CODE //////
            GameObject rowObj = CreateNewRowObj(rowIndex);
            ////////////////////////

            row = new List<Tile>();
            int columnIndex = 0;
            string[] line = reader.ReadLine().Split(',');

            foreach (string num in line) {
                GameObject tileObj = CreateTile(int.Parse(num), new Vector3(x, y, z), rowObj, rowIndex, columnIndex);
                Tile newTile = tileObj != null ? tileObj.GetComponent<Tile>() : null;
                row.Add(newTile);
                x += 2 * hexDimension.width;
                columnIndex++;
            }
            HexMap.mapArray.Add(row);
            y -= 2 * hexDimension.apex - hexDimension.minorApex;
            x -= 2 * hexDimension.width * line.Length - hexDimension.width;
            z -= .001f;
            rowIndex++;
        }
    }

    private GameObject CreateTile(int val, Vector3 pos, GameObject rowObj, int row, int col) {
        int type = val / 100;
        int variant = val % 100;
        GameObject tileObj = InstantiateTileFromPrefab(type, variant);
        if (tileObj != null) {
            tileObj.name = string.Format("Tile ({0}, {1})", row, col);
            tileObj.transform.parent = rowObj.transform;
            tileObj.transform.localPosition = pos;
            tileObj.GetComponent<Tile>().selectionController = battleMap.selectionController;
            TileLocation location = tileObj.GetComponent<TileLocation>();
            location.col = col;
            location.row = row;
        }
        return tileObj;
    }

    private GameObject InstantiateTileFromPrefab(int type, int variant) {
        switch(type) {
            case 0:
                {
                    // forest
                    GameObject obj = Instantiate(Resources.Load("Tiles/ForestTile")) as GameObject;
                    obj.GetComponent<SpriteRenderer>().sprite = spriteCache.GetTileSprite(type, variant);
                    return obj;
                }
            case 1:
                {
                    // grass
                    GameObject obj = Instantiate(Resources.Load("Tiles/GrassTile")) as GameObject;
                    obj.GetComponent<SpriteRenderer>().sprite = spriteCache.GetTileSprite(type, variant);
                    return obj;
                }
            case 2:
                {
                    // mountain
                    GameObject obj = Instantiate(Resources.Load("Tiles/MountainTile")) as GameObject;
                    obj.GetComponent<SpriteRenderer>().sprite = spriteCache.GetTileSprite(type, variant);
                    return obj;
                }
            case 3:
                {
                    // water
                    GameObject obj = Instantiate(Resources.Load("Tiles/WaterTile")) as GameObject;
                    obj.GetComponent<SpriteRenderer>().sprite = spriteCache.GetTileSprite(type, variant);
                    return obj;
                }
            default:
                return null;
        }
    }

    // Debug/organizational purposes - can be removed later
    private GameObject CreateNewRowObj(int rowIndex) {
        GameObject rowObj = new GameObject("Row " + rowIndex);
        rowObj.transform.parent = battleMap.transform;
        rowObj.transform.localPosition = new Vector3(0, 0, 0);
        return rowObj;
    }

    private void LoadUnits() {
        Vector2 faceRight = new Vector2(1, 0);
        Vector2 faceLeft = new Vector2(-1, 0);
        for(int i = 0; i < 4; i++) {
            AddUnitToTile(i, 0, true, faceRight);
            AddUnitToTile(i, 5, false, faceLeft);
        }
    }

    private void AddUnitToTile(int row, int col, bool isPlayerUnit, Vector3 facing) {
        GameObject unitObject = null;
        if (isPlayerUnit)
            unitObject = Instantiate(Resources.Load("Units/Rifleman")) as GameObject;
        else
            unitObject = Instantiate(Resources.Load("Units/Swordsman")) as GameObject;
        Unit unit = unitObject.GetComponent<Unit>();
        unit.SetTile(HexMap.mapArray[row][col]);
        unit.SetFacing(facing);
        if(!isPlayerUnit) {
            unitObject.AddComponent<BasicUnitAI>();
        }
    }
}

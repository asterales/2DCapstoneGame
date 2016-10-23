using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class GameResources {
    private static readonly string tileIdsFile = "TileIds";
    private static readonly string tileSpriteDir = "EditorSprites/Tiles/";
    private static Dictionary<int, List<Sprite>> tileSprites;
    private static Dictionary<int, GameObject> tilePrefabs;

    public static readonly Vector3 visibilityOffset = new Vector3(0, 0, -0.01f);
    public static readonly Vector3 hidingPosition = new Vector3(-1000, -1000, 0);

	static GameResources() {
        LoadTileCache();
	}

    private static void LoadTileCache() {
        Debug.Log("HI");
        tileSprites = new Dictionary<int, List<Sprite>>();
        tilePrefabs = new Dictionary<int, GameObject>();
        string[] tileIdLines = GetFileLines(tileSpriteDir + tileIdsFile);
        foreach(string line in tileIdLines) {
            string[] tokens = line.Split(',');
            if (tokens.Length != 2) continue;
            int id = int.Parse(tokens[0]);
            string folderName = tokens[1];
            tileSprites[id] = Resources.LoadAll<Sprite>(tileSpriteDir + folderName).ToList();
            tilePrefabs[id] = Resources.Load<GameObject>("Tiles/" + folderName + "Tile");
        }
    }

    public static Sprite GetTileSprite(int type, int variant) {
        return tileSprites[type][variant];
    }

    public static GameObject InstantiateTile(int type, int variant) {
        GameObject obj = UnityEngine.Object.Instantiate<GameObject>(tilePrefabs[type]);
        if (type != 4)
            obj.GetComponent<SpriteRenderer>().sprite = GetTileSprite(type, variant);
        return obj;
    }

    public static string[] GetFileLines(string filePathInResources) {
        TextAsset fileText = Resources.Load<TextAsset>(filePathInResources);
        if (fileText == null) {
            return null;
        }
        return fileText.text.Trim().Split('\n').Select(s => s.Trim()).ToArray();
    }
}
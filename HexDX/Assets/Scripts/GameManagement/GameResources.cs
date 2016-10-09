using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class GameResources {
    private static readonly string tileIdsFile = "TileIds";
    private static readonly string tileSpriteDir = "EditorSprites/Tiles/";
    private static Dictionary<int, List<Sprite>> tileSprites;
    private static Dictionary<int, GameObject> tilePrefabs;

	static GameResources() {
        LoadTileCache();
	}

    private static void LoadTileCache() {
        Debug.Log("Loaded tiles");
        tileSprites = new Dictionary<int, List<Sprite>>();
        tilePrefabs = new Dictionary<int, GameObject>();
        string[] tileIdLines = GetFileLines(tileSpriteDir + tileIdsFile);
        for (int i = 0; i < tileIdLines.Length; i++) {
            string[] tokens = tileIdLines[i].Split(',');
            int id = int.Parse(tokens[0]);
            string folderName = tokens[1];
            List<Sprite> variants = Resources.LoadAll<Sprite>(tileSpriteDir + folderName).ToList();
            tileSprites[id] = variants;
            tilePrefabs[id] = Resources.Load<GameObject>("Tiles/" + folderName + "Tile");
            Debug.Log(tilePrefabs[id].GetType());
        }
    }

    public static Sprite GetTileSprite(int type, int variant) {
        return tileSprites[type][variant];
    }

    public static GameObject InstantiateTile(int type, int variant) {
        GameObject obj = UnityEngine.Object.Instantiate<GameObject>(tilePrefabs[type]);
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
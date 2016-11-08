using UnityEngine;
using UnityEngine.UI;

public class TileUI : MonoBehaviour {

    public static Image tileImage;
    public static Text tileInfo;

	void Start () {
        tileImage = transform.Find("TileImage").GetComponent<Image>();
        tileInfo = transform.Find("TileInfo").GetComponent<Text>();
        tileImage.enabled = false;
    }
	
	void Update () {
	    if (SelectionController.selectedTile != null) {
            tileImage.enabled = true;
            tileImage.sprite = SelectionController.selectedTile.GetComponent<SpriteRenderer>().sprite;
            tileInfo.text = SelectionController.selectedTile.tileStats.DisplayString;
        }
    }
}

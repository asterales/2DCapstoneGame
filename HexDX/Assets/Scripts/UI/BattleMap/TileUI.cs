using UnityEngine;
using UnityEngine.UI;

public class TileUI : MonoBehaviour {

    private Image tileImage;
    private Text tileInfo;
    private SelectionController sc;

	void Start () {
        sc = SelectionController.instance;
        tileImage = transform.Find("TileImage").GetComponent<Image>();
        tileInfo = transform.Find("TileInfo").GetComponent<Text>();
        tileImage.enabled = false;
    }
	
	void Update () {
	    if (sc.selectedTile != null) {
            tileImage.enabled = true;
            tileImage.sprite = sc.selectedTile.GetComponent<SpriteRenderer>().sprite;
            tileInfo.text = sc.selectedTile.tileStats.DisplayString;
        }
    }
}

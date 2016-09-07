using UnityEngine;
using System.Collections;

// global controller object for the level editor.
// maybe in the future break this apart.

public class LEController : MonoBehaviour {
    public LESaveButton saveButton;
    public LELoadButton loadButton;
    public LEHexMap hexMap;
    public LEMapLoader mapLoader;
    public LEMapWriter mapWriter;
    public LESelectionController selectionController;
    public LESpriteCache spriteCache;
    public string fileName;

    void Start()
    {
        // save button is not a component of this object
        // load button is not a component of this object
        hexMap = this.gameObject.GetComponent<LEHexMap>();
        mapLoader = this.gameObject.GetComponent<LEMapLoader>();
        mapWriter = this.gameObject.GetComponent<LEMapWriter>();
        spriteCache = this.gameObject.GetComponent<LESpriteCache>();
        selectionController = this.gameObject.GetComponent<LESelectionController>();

        ////// DEBUG CODE //////
        if (saveButton == null)
        {
            Debug.Log("MAJOR ERROR :: Save Button Object not Linked -> LEController.cs");
        }
        if (loadButton == null)
        {
            Debug.Log("MAJOR ERROR :: Load Button Object not Linked -> LEController.cs");
        }
        if (hexMap == null)
        {
            Debug.Log("MAJOR ERROR :: HexMap Object not Defined -> LEController.cs");
        }
        if (mapLoader == null)
        {
            Debug.Log("MAJOR ERROR :: MapLoader Object not Defined -> LEController.cs");
        }
        if (mapWriter == null)
        {
            Debug.Log("MAJOR ERROR :: MapWriter Object not Defined -> LEController.cs");
        }
        if (selectionController == null)
        {
            Debug.Log("MAJOR ERROR :: SelectionController Object not Defined -> LEController.cs");
        }
        if (spriteCache == null)
        {
            Debug.Log("MAJOR ERROR :: Sprite Cache Object not Defined -> LEController.cs");
        }
        ////////////////////////

        // setting of connections between instance variables
        saveButton.mapWriter = mapWriter;
        loadButton.mapLoader = mapLoader;
        mapWriter.hexMap = hexMap;
        mapWriter.fileName = fileName;
        mapLoader.hexMap = hexMap;
        mapLoader.fileName = fileName;
        hexMap.spriteCache = spriteCache;
    }
}

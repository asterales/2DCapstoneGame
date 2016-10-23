using UnityEngine;
using System.Collections;

public class EndTurn : MonoBehaviour {
    public BattleController battleController;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        battleController = FindObjectOfType(typeof(BattleController)) as BattleController; //hack until figure out if static or manually attach
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        if (SelectionController.TakingInput()) {
            spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f);
            battleController.EndCurrentTurn();
        }

        if (SelectionController.mode == SelectionMode.ScriptedPlayerEndTurn) {
            spriteRenderer.color = new Color(0.3f, 0.3f, 0.3f);
            TutorialController.EndCurrentTurn();
        }
    }

    void OnMouseOver()
    {
        Camera.main.GetComponent<CameraController>().DontMove();
    }

    void OnMouseEnter()
    {
        spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f);
    }

    void OnMouseUp()
    {
        spriteRenderer.color = new Color(0.7f, 0.7f, 0.7f);
    }

    void OnMouseExit()
    {
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
    }
}

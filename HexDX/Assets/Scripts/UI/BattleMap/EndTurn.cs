using UnityEngine;

public class EndTurn : MonoBehaviour {
    private static Color hoverColor = new Color(0.7f, 0.7f, 0.7f);
    private static Color defaultColor = new Color(1.0f, 1.0f, 1.0f);
    private static Color clickColor = new Color(0.3f, 0.3f, 0.3f);

    public BattleController battleController;
    private SpriteRenderer spriteRenderer;

    void Start() {
        battleController = BattleManager.instance.battleController; //hack until figure out if static or manually attach
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitAlpha();
    }

    private void InitAlpha() {
        float alpha = spriteRenderer.color.a;
        hoverColor.a = alpha;
        defaultColor.a = alpha;
        clickColor.a = alpha;
    }

    void OnMouseDown() {
        if (SelectionController.instance.TakingInput()) {
            spriteRenderer.color = clickColor;
            battleController.EndCurrentTurn();
        }

        if (SelectionController.instance.mode == SelectionMode.ScriptedPlayerEndTurn) {
            spriteRenderer.color = clickColor;
            BattleManager.instance.tutorial.EndCurrentTurn();
        }
    }

    void OnMouseOver() {
        Camera.main.GetComponent<CameraController>().enabledMousePan = false;
    }

    void OnMouseEnter() {
        spriteRenderer.color = hoverColor;
    }

    void OnMouseUp() {
        spriteRenderer.color = hoverColor;
    }

    void OnMouseExit() {
        Camera.main.GetComponent<CameraController>().enabledMousePan = true;
        spriteRenderer.color = defaultColor;
    }
}

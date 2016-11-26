using UnityEngine;
using UnityEngine.UI;

public class EnemyUIDrawer : UnitUIDrawer {
    public static EnemyUIDrawer instance;

    public override void InitInstance() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    void Update() {
        if (sc.target && !sc.target.IsPlayerUnit()) {
            unit = sc.target;
        } else if (sc.selectedUnit != null && !sc.selectedUnit.IsPlayerUnit()) {
            unit = sc.selectedUnit;
        } else if (!BattleController.instance.IsPlayerTurn && BattleController.instance.ai.GetUnit() != null) {
            unit = BattleController.instance.ai.GetUnit();
        }
        DrawUI();
    }
}

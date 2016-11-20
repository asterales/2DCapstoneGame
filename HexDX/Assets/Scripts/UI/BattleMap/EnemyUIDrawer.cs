using UnityEngine;
using UnityEngine.UI;

public class EnemyUIDrawer : UnitUIDrawer {

    void Update() {
        if (sc.selectedUnit != null && !sc.selectedUnit.IsPlayerUnit()) {
            unit = sc.selectedUnit;
        }
        if (sc.target && !sc.target.IsPlayerUnit()) {
            unit = sc.target;
        }
        DrawUI();
    }
}

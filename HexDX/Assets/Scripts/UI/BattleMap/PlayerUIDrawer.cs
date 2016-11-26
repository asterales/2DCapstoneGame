using UnityEngine;
using UnityEngine.UI;

public class PlayerUIDrawer : UnitUIDrawer {
    public static PlayerUIDrawer instance;
    
    public override void InitInstance() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    void Update () {
	    if (sc.selectedUnit != null && sc.selectedUnit.IsPlayerUnit()) {
            unit = sc.selectedUnit;
        }
        if (sc.target && sc.target.IsPlayerUnit()) {
            unit = sc.target;
        }
        DrawUI();
    }
}

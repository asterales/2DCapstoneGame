using UnityEngine;

public class KillCommander: VictoryCondition {
    public Unit commander;
    private bool resized = false;
    void Update()
    {
        if (!resized && commander)
        {
            resized = true;
            commander.transform.localScale = new UnityEngine.Vector3(1.4f, 1.4f, 1.0f);
            GameObject g = Instantiate<GameObject>(Resources.Load<GameObject>("CommanderMark"));
            g.transform.SetParent(commander.transform);
            g.transform.localPosition = new Vector3(0, 0, 0.01f);
            g.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        victoryConditionText.text = "kill the commander!";
    }
    public override bool Achieved() {
        return !commander.gameObject.activeSelf; 
    }
}
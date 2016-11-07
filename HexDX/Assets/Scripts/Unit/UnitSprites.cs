using System.Collections;
using UnityEngine;

public class UnitSprites: MonoBehaviour
{
    // this class will eventually contain the stats of a tile
    public Sprite[] walking;
    public RuntimeAnimatorController[] walkingAnim;
    public Sprite[] idle;
    public RuntimeAnimatorController[] idleAnim;
    public Sprite[] attack;
    public RuntimeAnimatorController[] attackAnim;
    public Sprite portrait;
    public Sprite unitCard;
}

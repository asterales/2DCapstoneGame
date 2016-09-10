using UnityEngine;

// global class to help with expanding bars

public class LEExpansionController : MonoBehaviour {
    public static LEHorizonBar extendedBar;

    public static void DisableExpansion()
    {
        if (extendedBar != null)
        {
            extendedBar.DeExtend();
        }
        extendedBar = null;
    }
}

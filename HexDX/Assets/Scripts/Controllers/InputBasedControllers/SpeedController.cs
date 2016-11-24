using UnityEngine;
using System.Collections;

public class SpeedController : MonoBehaviour {
    private const float DEFAULT_SPEED = 1.0f;
    private const float ACCELERATED_SPEED = 4.0f;

    public static float speed = DEFAULT_SPEED;

	void Start () {
        speed = DEFAULT_SPEED;
	}
	
	void Update () {
        speed = Input.GetKey(KeyBindings.SPEED_UP) ? ACCELERATED_SPEED : DEFAULT_SPEED;	
	}
}

using UnityEngine;

public class Projectile : MonoBehaviour {
    public Vector3 start;
    public Vector3 stop;
    public float h;
    public float time;
    public float travelTime;

    private Matrix4x4 m;
    private Vector3 coeff;
    private float timestart;
	// Use this for initialization
	void Start () {
        transform.position = start;
        Vector3 midpoint = (start + stop)*0.5f+new Vector3(0, Vector2.Distance(start, stop)*h, 0);
        m.SetRow(0, new Vector4(start.x * start.x, start.x, 1, 0));
        m.SetRow(1, new Vector4(midpoint.x * midpoint.x, midpoint.x, 1, 0));
        m.SetRow(2, new Vector4(stop.x * stop.x, stop.x, 1, 0));
        m.SetRow(3, new Vector4(0, 0, 0, 1));
        coeff = m.inverse.MultiplyPoint3x4(new Vector3(start.y, midpoint.y, stop.y));
        timestart = Time.time;

    }

    private float x(float t)
    {
        return start.x * (1-t / travelTime) + stop.x * (t / travelTime);
    }
	
    private Vector3 position(float x)
    {
        return new Vector3(x, coeff.x * x * x+coeff.y * x+coeff.z, 1);
    }

    private Quaternion rotation(float x)
    {
        float angle = Angle(Vector2.zero, new Vector2(1, 2.0f * coeff.x * x + coeff.y));
        Debug.Log(angle);
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private float Angle(Vector2 from, Vector2 to)
    {
        Vector2 diff = to - from;
        float output = (Mathf.Atan2(diff.y, diff.x) * 57.2957795131f);
        return output;
    }
    // Update is called once per frame
    void Update () {
        if (Time.time - timestart < travelTime)
        {
            transform.position = position(x(Time.time - timestart));
            transform.rotation = rotation(x(Time.time - timestart));
        }
        if (Time.time-timestart>time)
            GameObject.Destroy(this.gameObject);
	}
}
